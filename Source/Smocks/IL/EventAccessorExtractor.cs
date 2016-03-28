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
using Smocks.IL.Resolvers;
using Smocks.Setups;
using Smocks.Utility;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace Smocks.IL
{
    internal class EventAccessorExtractor : IEventAccessorExtractor
    {
        private readonly IInstructionHelper _instructionHelper;
        private readonly IInstructionsCompiler _instructionsCompiler;
        private readonly IMethodDisassembler _methodDisassembler;
        private readonly IMethodResolver _methodResolver;
        private readonly IParameterDeducer _parameterDeducer;

        public EventAccessorExtractor(
            IMethodDisassembler methodDisassembler,
            IInstructionHelper instructionHelper,
            IMethodResolver methodResolver,
            IInstructionsCompiler instructionsCompiler,
            IParameterDeducer parameterDeducer)
        {
            ArgumentChecker.NotNull(methodDisassembler, nameof(methodDisassembler));
            ArgumentChecker.NotNull(instructionHelper, nameof(instructionHelper));
            ArgumentChecker.NotNull(methodResolver, nameof(methodResolver));
            ArgumentChecker.NotNull(instructionsCompiler, nameof(instructionsCompiler));
            ArgumentChecker.NotNull(parameterDeducer, nameof(parameterDeducer));

            _methodDisassembler = methodDisassembler;
            _instructionHelper = instructionHelper;
            _methodResolver = methodResolver;
            _instructionsCompiler = instructionsCompiler;
            _parameterDeducer = parameterDeducer;
        }

        private enum AccessorKind
        {
            Add,
            Remove
        }

        public EventAccessorPair FindEventAccessor(Action addAction, Action removeAction)
        {
            List<Tuple<MethodBase, object>> addAccessors = FindEventAddRemoveAccessors(addAction).ToList();
            AssertAccessorCount(addAccessors.Count, AccessorKind.Add, nameof(addAction));

            List<Tuple<MethodBase, object>> removeAccessors = FindEventAddRemoveAccessors(removeAction).ToList();
            AssertAccessorCount(addAccessors.Count, AccessorKind.Remove, nameof(removeAction));

            // Targets must match
            if (!Equals(addAccessors[0].Item2, removeAccessors[0].Item2))
            {
                throw new ArgumentException("Targets of add/remove accessors do not match");
            }

            return new EventAccessorPair(addAccessors[0].Item2, addAccessors[0].Item1, removeAccessors[0].Item1);
        }

        private void AssertAccessorCount(int count, AccessorKind accessorKind, string parameterName)
        {
            if (count == 0)
            {
                throw new ArgumentException(string.Format("Could not find {0} accessor", accessorKind), parameterName);
            }

            if (count > 1)
            {
                throw new ArgumentException(string.Format("Found more than one {0} accessor", accessorKind), parameterName);
            }
        }

        private IEnumerable<Tuple<MethodBase, object>> FindEventAddRemoveAccessors(Action action)
        {
            DisassembleResult disassembleResult = _methodDisassembler.Disassemble(action.Method);

            foreach (Instruction instruction in disassembleResult.Body.Instructions)
            {
                MethodReference calledMethod;
                if (_instructionHelper.TryGetCall(instruction, out calledMethod))
                {
                    if (IsEventAddRemoveMethod(calledMethod))
                    {
                        object target = calledMethod.HasThis
                            ? GetInvocationTarget(disassembleResult.Body, instruction, action.Target)
                            : null;

                        MethodBase method = _methodResolver.Resolve(calledMethod);

                        yield return Tuple.Create(method, target);
                    }
                }
            }
        }

        private object GetInvocationTarget(MethodBody body, Instruction callInstruction, object outerTarget)
        {
            var instructions = body.Instructions.ToList();
            var index = instructions.IndexOf(callInstruction);
            instructions.RemoveRange(index, instructions.Count - index);
            instructions.Add(Instruction.Create(OpCodes.Pop));
            instructions.Add(Instruction.Create(OpCodes.Ret));

            // Gets the parameters used by the instructions.
            TypeReference[] parameterTypes = _parameterDeducer.GetParameters(body.Method, instructions);

            object arg = _instructionsCompiler
                .Compile<object>(parameterTypes, instructions, new VariableDefinition[0])
                .Invoke(new[] { outerTarget });

            return arg;
        }

        private bool IsEventAddRemoveMethod(MethodReference method)
        {
            bool result = false;

            if (method.Parameters.Count == 1)
            {
                TypeDefinition parameterType = method.Parameters[0].ParameterType.Resolve();
                result = IsMulticastDelegate(parameterType);
            }

            return result;
        }

        private bool IsMulticastDelegate(TypeDefinition parameterType)
        {
            return parameterType != null &&
                (parameterType.FullName == "System.MulticastDelegate" || IsMulticastDelegate(parameterType.BaseType.Resolve()));
        }
    }
}