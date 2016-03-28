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

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.Exceptions;
using Smocks.Setups;
using Smocks.Utility;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace Smocks.IL
{
    internal class SetupExtractor : ISetupExtractor
    {
        private readonly IExpressionDecompiler<Expression> _expressionDecompiler;
        private readonly IExpressionHelper _expressionHelper;
        private readonly IMethodDisassembler _methodDisassembler;
        private readonly List<MethodInfo> _setupMethods = GetSetupMethods().ToList();

        internal SetupExtractor(
            IMethodDisassembler methodDisassembler,
            IExpressionDecompiler<Expression> expressionDecompiler,
            IExpressionHelper expressionHelper)
        {
            ArgumentChecker.NotNull(methodDisassembler, () => methodDisassembler);
            ArgumentChecker.NotNull(expressionDecompiler, () => expressionDecompiler);
            ArgumentChecker.NotNull(expressionHelper, () => expressionHelper);

            _methodDisassembler = methodDisassembler;
            _expressionDecompiler = expressionDecompiler;
            _expressionHelper = expressionHelper;
        }

        public IEnumerable<SetupTarget> GetSetups(MethodBase method, object target)
        {
            DisassembleResult disassembleResult = _methodDisassembler.Disassemble(method);

            List<MethodDefinition> setupMethods = _setupMethods.Select(setupMethod =>
                disassembleResult.ModuleDefinition.Import(setupMethod).Resolve()).ToList();

            return GetSetupsFromInstructions(target, disassembleResult.Body, setupMethods);
        }

        public IEnumerable<SetupTarget> GetSetups(MethodBase method)
        {
            return GetSetups(method, null);
        }

        private static TypeDefinition GetRoot(TypeDefinition type)
        {
            TypeDefinition result = type;

            while (result.DeclaringType != null)
            {
                result = result.DeclaringType;
            }

            return result;
        }

        private static IEnumerable<MethodInfo> GetSetupMethods()
        {
            return typeof(ISmocksContext).GetMethods().Where(method => method.Name.Equals("Setup"));
        }

        private static bool InSameClass(TypeDefinition first, TypeDefinition second)
        {
            return GetRoot(first) == GetRoot(second);
        }

        private IEnumerable<SetupTarget> GetSetupsFromInstructions(object target, MethodBody body, List<MethodDefinition> setupMethods)
        {
            foreach (var instruction in body.Instructions)
            {
                if (instruction.OpCode != OpCodes.Callvirt && instruction.OpCode != OpCodes.Call)
                {
                    continue;
                }

                var operandMethod = ((MethodReference)instruction.Operand).Resolve();

                if (setupMethods.Contains(operandMethod))
                {
                    Expression expression = _expressionDecompiler.Decompile(body, instruction, target);

                    if (expression == null)
                    {
                        // This should not happen. When this happens it's most likely
                        // a bug in the expression decompiler.
                        throw new SetupExtractionException("Could not extract expression");
                    }

                    var methodCall = _expressionHelper.GetMethod(expression);

                    if (methodCall == null)
                    {
                        string message = string.Format(
                            "Could not extract method from expression {0}", expression);
                        throw new SetupExtractionException(message);
                    }

                    yield return new SetupTarget(expression, methodCall.Method);
                }
                else
                {
                    bool inSameClass = InSameClass(operandMethod.DeclaringType, body.Method.DeclaringType);
                    if (inSameClass && operandMethod != body.Method && operandMethod.Body != null)
                    {
                        foreach (var setup in GetSetupsFromInstructions(target, operandMethod.Body, setupMethods))
                        {
                            yield return setup;
                        }
                    }
                }
            }
        }
    }
}