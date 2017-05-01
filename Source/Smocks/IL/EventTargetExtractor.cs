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
using Smocks.Setups;
using Smocks.Utility;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace Smocks.IL
{
    internal class EventTargetExtractor : IEventTargetExtractor
    {
        private readonly IEventAccessorExtractor _eventAccessorExtractor;
        private readonly IExpressionDecompiler<Action> _expressionDecompiler;
        private readonly IInstructionHelper _instructionHelper;
        private readonly IMethodDisassembler _methodDisassembler;

        private readonly List<MethodInfo> _setupMethods = GetSetupMethods().ToList();

        public EventTargetExtractor(
            IMethodDisassembler methodDisassembler,
            IInstructionHelper instructionHelper,
            IExpressionDecompiler<Action> expressionDecompiler,
            IEventAccessorExtractor eventAccessorExtractor)
        {
            ArgumentChecker.NotNull(methodDisassembler, nameof(methodDisassembler));
            ArgumentChecker.NotNull(instructionHelper, nameof(instructionHelper));
            ArgumentChecker.NotNull(expressionDecompiler, nameof(expressionDecompiler));
            ArgumentChecker.NotNull(eventAccessorExtractor, nameof(eventAccessorExtractor));

            _methodDisassembler = methodDisassembler;
            _instructionHelper = instructionHelper;
            _expressionDecompiler = expressionDecompiler;
            _eventAccessorExtractor = eventAccessorExtractor;
        }

        public IEnumerable<IRewriteTarget> GetTargets(MethodBase method, object target)
        {
            using (DisassembleResult disassembleResult = _methodDisassembler.Disassemble(method))
            {
                List<MethodDefinition> setupMethods = _setupMethods.Select(setupMethod =>
                    disassembleResult.ModuleDefinition.ImportReference(setupMethod).Resolve()).ToList();

                return GetSetupsFromInstructions(target, disassembleResult.Body, setupMethods);
            }
        }

        private static IEnumerable<MethodInfo> GetSetupMethods()
        {
            return typeof(ISmocksContext).GetMethods().Where(method => method.Name.Equals(nameof(ISmocksContext.Raise)));
        }

        private IEnumerable<IRewriteTarget> GetSetupsFromInstructions(object target, MethodBody body, List<MethodDefinition> setupMethods)
        {
            var result = new List<IRewriteTarget>();

            foreach (var instruction in body.Instructions)
            {
                MethodReference calledMethod;
                if (_instructionHelper.TryGetCall(instruction, out calledMethod))
                {
                    var operandMethod = calledMethod.Resolve();
                    if (setupMethods.Contains(operandMethod))
                    {
                        const int expectedStackSize = 3;

                        // Raise(addAction, removeAction, argument) --> skip 1 for removeAction, skip 2 for addAction.
                        Action addAction = _expressionDecompiler.Decompile(body, instruction, target, expectedStackSize, 2);
                        Action removeAction = _expressionDecompiler.Decompile(body, instruction, target, expectedStackSize, 1);

                        EventAccessorPair accessorPair = _eventAccessorExtractor.FindEventAccessor(addAction, removeAction);

                        Type addMethodEventHandlerType = accessorPair.AddAccessor.GetParameters()[0].ParameterType;
                        Type removeMethodEventHandlerType = accessorPair.RemoveAccessor.GetParameters()[0].ParameterType;

                        if (addMethodEventHandlerType != removeMethodEventHandlerType)
                        {
                            throw new ArgumentException("Event handler types do not match");
                        }

                        result.Add(new EventRewriteTarget(accessorPair.AddAccessor, accessorPair.RemoveAccessor, addMethodEventHandlerType));
                    }
                }
            }

            return result;
        }
    }
}