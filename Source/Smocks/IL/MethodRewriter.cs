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
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.IL.Resolvers;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks.IL
{
    internal class MethodRewriter : IMethodRewriter
    {
        private const string CecilConstructorName = ".ctor";

        private static readonly MethodBase GetMethodFromHandleMethod =
            typeof(MethodBase).GetMethod("GetMethodFromHandle", new[] { typeof(RuntimeMethodHandle) });

        private static readonly MethodBase HookMethod = typeof(Interceptor).GetMethod("Intercept");
        private static readonly MethodBase HookVoidMethod = typeof(Interceptor).GetMethod("InterceptVoid");
        private readonly IInstructionHelper _instructionHelper;

        internal MethodRewriter(IInstructionHelper instructionHelper)
        {
            ArgumentChecker.NotNull(instructionHelper, () => instructionHelper);

            _instructionHelper = instructionHelper;
        }

        public bool Rewrite(Configuration configuration, MethodDefinition method,
            ISetupTargetMatcher setupTargetMatcher)
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
                    var targets = setupTargetMatcher.GetMatchingTargets(calledMethod).ToList();

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

        private static void CreateArgumentsToArrayInstructions(ILProcessor processor,
            MethodReference targetMethod, Instruction replacementInstruction)
        {
            var parameters = GetParameters(targetMethod);

            VariableDefinition arrayVariable =
                new VariableDefinition(targetMethod.Module.Import(typeof(Object[])));
            VariableDefinition arrayElementVariable =
                new VariableDefinition(targetMethod.Module.Import(typeof(Object)));

            processor.Body.Variables.Add(arrayVariable);
            processor.Body.Variables.Add(arrayElementVariable);

            processor.InsertBefore(replacementInstruction,
                Instruction.Create(OpCodes.Ldc_I4, parameters.Count));
            processor.InsertBefore(replacementInstruction,
                Instruction.Create(OpCodes.Newarr, targetMethod.Module.Import(typeof(Object))));

            processor.InsertBefore(replacementInstruction,
                Instruction.Create(OpCodes.Stloc, arrayVariable));

            // Add items on stack to array. Note that we pop them backwards, so
            // we add them backwards as well.
            for (int i = parameters.Count - 1; i >= 0; --i)
            {
                TypeReference parameterType = ResolveGenericParameters(parameters[i], targetMethod);

                if (parameterType.IsValueType)
                {
                    // If the target of the method is a value type, it's *address* is pushed on the stack
                    // and we need to dereference it using ldobj to get the actual value.
                    if (targetMethod.HasThis && i == 0)
                    {
                        processor.InsertBefore(replacementInstruction, Instruction.Create(OpCodes.Ldobj, parameterType));
                    }

                    processor.InsertBefore(replacementInstruction, Instruction.Create(OpCodes.Box, parameterType));
                }

                processor.InsertBefore(replacementInstruction,
                    Instruction.Create(OpCodes.Stloc, arrayElementVariable));

                processor.InsertBefore(replacementInstruction,
                    Instruction.Create(OpCodes.Ldloc, arrayVariable));
                processor.InsertBefore(replacementInstruction,
                    Instruction.Create(OpCodes.Ldc_I4, i));
                processor.InsertBefore(replacementInstruction,
                    Instruction.Create(OpCodes.Ldloc, arrayElementVariable));
                processor.InsertBefore(replacementInstruction,
                    Instruction.Create(OpCodes.Stelem_Ref));
            }

            processor.InsertBefore(replacementInstruction,
                    Instruction.Create(OpCodes.Ldloc, arrayVariable));
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

        private static bool IsConstructor(MethodReference method)
        {
            return method.Name == CecilConstructorName;
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

        private void CreatePushTargetMethodOnStackInstructions(ILProcessor processor,
            MethodReference targetMethod, Instruction replacementInstruction)
        {
            processor.InsertBefore(replacementInstruction,
                Instruction.Create(OpCodes.Ldtoken, targetMethod));
            processor.InsertBefore(replacementInstruction,
                Instruction.Create(OpCodes.Call, targetMethod.Module.Import(GetMethodFromHandleMethod)));
        }

        private void RewriteInstruction(RewriteContext context)
        {
            bool isVoidMethod = context.Method.ReturnType.FullName == "System.Void" &&
                !IsConstructor(context.Method);

            MethodBase replacementMethod = isVoidMethod ? HookVoidMethod : HookMethod;
            MethodReference importedReplacement = context.Method.Module.Import(replacementMethod);

            bool isGenericHookMethod = importedReplacement.HasGenericParameters;

            if (isGenericHookMethod)
            {
                GenericInstanceMethod boundMethod = new GenericInstanceMethod(importedReplacement);

                TypeReference returnValue = IsConstructor(context.Method)
                    ? context.Method.DeclaringType
                    : context.Method.ReturnType;

                returnValue = ResolveGenericParameters(returnValue, context.Method);

                boundMethod.GenericArguments.Add(returnValue);

                importedReplacement = boundMethod;
            }

            Instruction replacementInstruction = Instruction.Create(OpCodes.Call, importedReplacement);
            context.Processor.Replace(context.OriginalInstruction, replacementInstruction);

            CreateArgumentsToArrayInstructions(context.Processor, context.Method, replacementInstruction);
            CreatePushTargetMethodOnStackInstructions(context.Processor, context.Method, replacementInstruction);
        }
    }
}