#region License
//// The MIT License (MIT)
////
//// Copyright (c) 2015 Tom van der Kleij
////
//// Permission is hereby granted, free of charge, to any person obtaining a copy of
//// this software and associated documentation files (the "Software"), to deal in
//// the Software without restriction, including without limitation the rights to
//// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//// the Software, and to permit persons to whom the Software is furnished to do so,
//// subject to the following conditions:
////
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
////
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Smocks.IL.Resolvers;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks.IL
{
    internal class MethodRewriter : IMethodRewriter
    {
        private const string CecilConstructorName = ".ctor";

        private static readonly MethodBase GetMethodFromHandleMethod =
            typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle), new[] { typeof(RuntimeMethodHandle) });

        private static readonly MethodBase HookMethod = typeof(Interceptor).GetMethod(nameof(Interceptor.Intercept));

        private static readonly MethodBase HookVoidEventMethod = typeof(EventInterceptor).GetMethod(nameof(EventInterceptor.InterceptVoidEvent));
        private static readonly MethodBase HookVoidMethod = typeof(Interceptor).GetMethod(nameof(Interceptor.InterceptVoid));
        private static readonly PropertyInfo InterceptedProperty = typeof(InterceptorResult).GetProperty(nameof(InterceptorResult.Intercepted));

        private readonly IInstructionHelper _instructionHelper;

        internal MethodRewriter(IInstructionHelper instructionHelper)
        {
            ArgumentChecker.NotNull(instructionHelper, () => instructionHelper);

            _instructionHelper = instructionHelper;
        }

        public bool Rewrite(Configuration configuration, MethodDefinition method,
            IRewriteTargetMatcher rewriteTargetMatcher)
        {
            bool rewritten = false;

            if (method.Body == null)
            {
                return rewritten;
            }

            ILProcessor processor = method.Body.GetILProcessor();

            // Iterate copy of initial instructions without modifications
            foreach (var instruction in method.Body.Instructions.ToList())
            {
                MethodReference calledMethod;
                if (_instructionHelper.TryGetCall(instruction, out calledMethod))
                {
                    List<IRewriteTarget> targets = rewriteTargetMatcher.GetMatchingTargets(calledMethod).ToList();

                    if (targets.Count == 0)
                    {
                        continue;
                    }

                    RewriteContext context = new RewriteContext(configuration,
                        processor, instruction, targets, calledMethod);
                    RewriteInstruction(context);

                    rewritten = true;
                }
            }

            return rewritten;
        }

        private static MethodReference GetInterceptorMethod(MethodReference originalMethod, bool isVoidMethod, EventRewriteTarget eventTarget)
        {
            MethodBase replacementMethod = GetReplacementMethod(isVoidMethod, isEvent: eventTarget != null);
            MethodReference importedReplacement = originalMethod.Module.ImportReference(replacementMethod);

            bool isGenericHookMethod = importedReplacement.HasGenericParameters;

            if (isGenericHookMethod)
            {
                GenericInstanceMethod boundMethod = new GenericInstanceMethod(importedReplacement);

                if (eventTarget != null)
                {
                    boundMethod.GenericArguments.Add(originalMethod.Module.ImportReference(eventTarget.EventHandlerType));
                }

                if (!isVoidMethod)
                {
                    TypeReference returnValue = GetReturnValue(originalMethod);
                    boundMethod.GenericArguments.Add(returnValue);
                }

                importedReplacement = boundMethod;
            }

            return importedReplacement;
        }

        private static List<TypeReference> GetParameters(MethodReference method)
        {
            List<TypeReference> parameterTypes = new List<TypeReference>();

            if (method.HasThis && !IsConstructor(method))
            {
                parameterTypes.Add(method.DeclaringType);
            }

            parameterTypes.AddRange(method.Parameters.Select(parameter => parameter.ParameterType));

            return parameterTypes;
        }

        private static MethodBase GetReplacementMethod(bool isVoidMethod, bool isEvent)
        {
            if (isEvent && !isVoidMethod)
            {
                throw new InvalidOperationException("An event accessor should always return void");
            }

            return isEvent
                ? HookVoidEventMethod
                : (isVoidMethod ? HookVoidMethod : HookMethod);
        }

        private static TypeReference GetReturnValue(MethodReference originalMethod)
        {
            TypeReference returnValue = IsConstructor(originalMethod)
                ? originalMethod.DeclaringType
                : originalMethod.ReturnType;

            returnValue = ResolveGenericParameters(returnValue, originalMethod);
            return returnValue;
        }

        private static bool IsConstructor(MethodReference method)
        {
            return method.Name == CecilConstructorName;
        }

        private static void PushMethod(RewriteContext context, MethodReference method)
        {
            context.Insert(Instruction.Create(OpCodes.Ldtoken, method));
            context.Insert(Instruction.Create(OpCodes.Call, context.Method.Module.ImportReference(GetMethodFromHandleMethod)));
        }

        private static TypeReference ResolveGenericParameters(TypeReference typeReference, MethodReference method)
        {
            GenericInstanceMethod genericInstanceMethod = method as GenericInstanceMethod;

            if (genericInstanceMethod != null)
            {
                GenericBindingContext context = GenericBindingContext.Create(genericInstanceMethod);
                typeReference = context.Resolve(typeReference);
            }

            return typeReference;
        }

        private static TypeReference StripByReference(RewriteContext context, TypeReference type)
        {
            return context.Method.Module.ImportReference(type.Resolve());
        }

        private void CopyReferenceParametersToAddress(RewriteContext context,
            VariableDefinition arrayVariable, List<VariableDefinition> variables)
        {
            for (int i = 0; i < variables.Count; ++i)
            {
                if (variables[i].VariableType.IsByReference)
                {
                    var variableType = StripByReference(context, variables[i].VariableType);

                    // Push address
                    context.Insert(Instruction.Create(OpCodes.Ldloc, variables[i]));

                    // Push value from arguments array
                    context.Insert(Instruction.Create(OpCodes.Ldloc, arrayVariable));
                    context.Insert(Instruction.Create(OpCodes.Ldc_I4, i));
                    context.Insert(Instruction.Create(OpCodes.Ldelem_Any, context.Method.Module.ImportReference(typeof(object))));
                    context.Insert(Instruction.Create(OpCodes.Unbox_Any, variableType));

                    // Copy value to address
                    context.Insert(Instruction.Create(OpCodes.Stind_Ref));
                }
            }
        }

        private TypeReference GetInterceptorResultType(MethodReference method, bool isVoidMethod)
        {
            if (isVoidMethod)
            {
                return method.Module.ImportReference(typeof(InterceptorResult));
            }

            TypeReference unbound = method.Module.ImportReference(typeof(InterceptorResult<>));

            var bound = new GenericInstanceType(unbound);

            TypeReference returnValue = GetReturnValue(method);
            bound.GenericArguments.Add(returnValue);

            return bound;
        }

        private Instruction InvokeOriginalMethod(RewriteContext context, List<VariableDefinition> variables)
        {
            Instruction instruction;
            Instruction firstInstruction = null;

            for (int i = 0; i < variables.Count; ++i)
            {
                var variable = variables[i];

                // If we're calling an instance method of a value type, we need to put the
                // *address* of the instance on the stack.
                if (i == 0 && context.Method.HasThis && context.Method.DeclaringType.IsValueType)
                {
                    instruction = context.Insert(Instruction.Create(OpCodes.Ldloca, variable));
                }
                else
                {
                    instruction = context.Insert(Instruction.Create(OpCodes.Ldloc, variable));
                }

                firstInstruction = firstInstruction ?? instruction;
            }

            instruction = context.Insert(context.OriginalInstruction);
            firstInstruction = firstInstruction ?? instruction;

            return firstInstruction;
        }

        private VariableDefinition InvokeReplacementMethod(RewriteContext context, VariableDefinition arrayVariable, bool isVoidMethod)
        {
            EventRewriteTarget eventTarget = context.Targets[0] as EventRewriteTarget;

            var importedReplacement = GetInterceptorMethod(context.Method, isVoidMethod, eventTarget);

            context.Insert(Instruction.Create(OpCodes.Ldloc, arrayVariable));

            PushMethod(context, context.Method);

            if (eventTarget != null)
            {
                PushMethod(context, context.Method.Module.ImportReference(eventTarget.AddMethod));
                PushMethod(context, context.Method.Module.ImportReference(eventTarget.RemoveMethod));
            }

            context.Insert(Instruction.Create(OpCodes.Call, importedReplacement));

            var interceptorResultVariable = new VariableDefinition(GetInterceptorResultType(context.Method, isVoidMethod));
            context.Processor.Body.Variables.Add(interceptorResultVariable);

            context.Insert(Instruction.Create(OpCodes.Stloc, interceptorResultVariable));

            return interceptorResultVariable;
        }

        private void PushReturnValueOnStack(RewriteContext context, VariableDefinition interceptorResultVariable, bool isVoidMethod)
        {
            if (!isVoidMethod)
            {
                var type = context.Method.Module.ImportReference(typeof(InterceptorResult<>));
                var genericType = new GenericInstanceType(type);
                genericType.GenericArguments.Add(GetReturnValue(context.Method));

                var returnValueProperty = type.Resolve().Properties.First(property => property.Name == "ReturnValue");
                var getMethod = context.Method.Module.ImportReference(returnValueProperty.GetMethod);
                getMethod.DeclaringType = genericType;

                context.Insert(Instruction.Create(OpCodes.Ldloc, interceptorResultVariable));
                context.Insert(Instruction.Create(OpCodes.Callvirt, getMethod));
            }
        }

        private void RewriteInstruction(RewriteContext context)
        {
            bool isVoidMethod = context.Method.ReturnType.FullName == "System.Void" &&
                !IsConstructor(context.Method);

            // As per: https://groups.google.com/forum/#!topic/mono-cecil/v6LfFLj9eAE
            context.Processor.Body.SimplifyMacros();

            List<VariableDefinition> variables = StoreArgumentsInVariables(context);
            VariableDefinition arrayVariable = StoreVariablesInObjectArray(context, variables);
            VariableDefinition interceptorResultVariable = InvokeReplacementMethod(context, arrayVariable, isVoidMethod);

            Instruction currentPosition = context.CurrentPosition;

            // If the method was intercepted, we need to push the return value on the stack.
            PushReturnValueOnStack(context, interceptorResultVariable, isVoidMethod);

            // And copy any out/reference parameters back to their original address.
            CopyReferenceParametersToAddress(context, arrayVariable, variables);

            // Then skip over the part for when we didn't intercept.
            context.Insert(Instruction.Create(OpCodes.Br, context.OriginalInstruction.Next));

            // If it wasn't we need to rebuild the stack and invoke the original method.
            Instruction invokeOriginalStartInstruction = InvokeOriginalMethod(context, variables);

            // Inject an instruction to jump based on whether the instruction was intercepted.
            var interceptedPropertyGetter = context.Method.Module.ImportReference(InterceptedProperty.GetGetMethod());
            context.InsertAfter(currentPosition, new[]
            {
                Instruction.Create(OpCodes.Ldloc, interceptorResultVariable),
                Instruction.Create(OpCodes.Callvirt, interceptedPropertyGetter),
                Instruction.Create(OpCodes.Brfalse, invokeOriginalStartInstruction)
            });

            context.Processor.Remove(context.OriginalInstruction);

            context.Processor.Body.OptimizeMacros();
        }

        private List<VariableDefinition> StoreArgumentsInVariables(RewriteContext context)
        {
            List<TypeReference> parameters = GetParameters(context.Method);
            List<VariableDefinition> variables = parameters
                .Select(parameterType => new VariableDefinition(ResolveGenericParameters(parameterType, context.Method)))
                .ToList();

            context.AddVariables(variables);

            // Add items on stack to array. Note that we pop them backwards, so
            // we add them backwards as well.
            for (int i = parameters.Count - 1; i >= 0; --i)
            {
                TypeReference parameterType = ResolveGenericParameters(parameters[i], context.Method);

                if (parameterType.IsByReference)
                {
                }
                else if (parameterType.IsValueType)
                {
                    // If the declaring type of the method is a value type, it's *address* is pushed on the stack
                    // and we need to dereference it using ldobj to get the actual value.
                    if (context.Method.HasThis && i == 0)
                    {
                        context.Insert(Instruction.Create(OpCodes.Ldobj, parameterType));
                    }
                }

                context.Insert(Instruction.Create(OpCodes.Stloc, variables[i]));
            }

            return variables;
        }

        private VariableDefinition StoreVariablesInObjectArray(RewriteContext context, List<VariableDefinition> variables)
        {
            var arrayVariable = new VariableDefinition(context.Method.Module.ImportReference(typeof(Object[])));
            context.Processor.Body.Variables.Add(arrayVariable);

            context.Insert(Instruction.Create(OpCodes.Ldc_I4, variables.Count));
            context.Insert(Instruction.Create(OpCodes.Newarr, context.Method.Module.ImportReference(typeof(Object))));

            context.Insert(Instruction.Create(OpCodes.Stloc, arrayVariable));

            for (int i = 0; i < variables.Count; ++i)
            {
                VariableDefinition variable = variables[i];
                TypeReference variableType = variable.VariableType;

                context.Insert(Instruction.Create(OpCodes.Ldloc, arrayVariable));
                context.Insert(Instruction.Create(OpCodes.Ldc_I4, i));
                context.Insert(Instruction.Create(OpCodes.Ldloc, variable));

                if (variableType.IsByReference)
                {
                    variableType = StripByReference(context, variable.VariableType);
                    context.Insert(Instruction.Create(OpCodes.Ldobj, variableType));
                }

                if (variableType.IsValueType)
                {
                    context.Insert(Instruction.Create(OpCodes.Box, variableType));
                }

                context.Insert(Instruction.Create(OpCodes.Stelem_Ref));
            }

            return arrayVariable;
        }
    }
}