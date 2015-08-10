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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.IL.Resolvers;
using Smocks.IL.Visitors;
using Smocks.Utility;

namespace Smocks.IL
{
    /// <summary>
    /// Tries to "decompile" an expression from a set of instructions
    /// by extracting all instructions relevant to the the construction
    /// of the expression, recompiling those instructions to a method
    /// and invoking this new method.
    /// </summary>
    internal class ExpressionDecompiler : IExpressionDecompiler
    {
        private readonly IArgumentGenerator _argumentGenerator;
        private readonly IInstructionHelper _instructionHelper;
        private readonly IInstructionsCompiler _instructionsCompiler;
        private readonly IParameterDeducer _parameterDeducer;
        private readonly ITypeResolver _typeResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionDecompiler"/> class.
        /// </summary>
        /// <param name="instructionsCompiler">The instructions compiler.</param>
        /// <param name="instructionHelper">The instruction helper.</param>
        /// <param name="parameterDeducer">The parameter deducer.</param>
        /// <param name="argumentGenerator">The argument generator.</param>
        /// <param name="typeResolver">The type resolver.</param>
        internal ExpressionDecompiler(
            IInstructionsCompiler instructionsCompiler,
            IInstructionHelper instructionHelper,
            IParameterDeducer parameterDeducer,
            IArgumentGenerator argumentGenerator,
            ITypeResolver typeResolver)
        {
            ArgumentChecker.NotNull(instructionsCompiler, () => instructionsCompiler);
            ArgumentChecker.NotNull(instructionHelper, () => instructionHelper);
            ArgumentChecker.NotNull(parameterDeducer, () => parameterDeducer);
            ArgumentChecker.NotNull(argumentGenerator, () => argumentGenerator);
            ArgumentChecker.NotNull(typeResolver, () => typeResolver);

            _instructionsCompiler = instructionsCompiler;
            _instructionHelper = instructionHelper;
            _parameterDeducer = parameterDeducer;
            _argumentGenerator = argumentGenerator;
            _typeResolver = typeResolver;
        }

        /// <summary>
        /// Decompiles an expression that's on the stack at the specified
        /// instruction in the specified method. This is done by reversing from the
        /// specified instruction until the stack should be empty. The instructions
        /// can then be replayed from that point.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="instruction">The instruction.</param>
        /// <param name="target">The target of the method, if any.</param>
        /// <returns>The compiled expression.</returns>
        public Expression Decompile(MethodBody body, Instruction instruction, object target)
        {
            BitArray includedInstructions = new BitArray(body.Instructions.Count);

            if (IncludeInstructionsUntilStackEmpty(body, instruction, includedInstructions))
            {
                List<Instruction> instructions = body.Instructions
                        .Where((t, i) => includedInstructions[i])
                        .ToList();

                // Gets the parameters used by the instructions.
                TypeReference[] parameterTypes = _parameterDeducer.GetParameters(body.Method, instructions);
                Type[] resolvedParameterTypes = parameterTypes.Select(_typeResolver.Resolve).ToArray();

                // TODO: pass only the used variables instead of Body.Variables.
                // Currently, this is bugged so body.Variables is used. The additional
                // variables are not really an issue.
                ICompiledMethod<Expression> method = _instructionsCompiler.Compile<Expression>(
                    parameterTypes, instructions, body.Variables);

                // Try to construct the arguments required by the instructions.
                object[] arguments = _argumentGenerator.GetArguments(resolvedParameterTypes, target).ToArray();

                return method.Invoke(arguments);
            }

            return null;
        }

        private static int GetNumberPoppedByInstruction(Instruction current)
        {
            return current.Accept(new NumberPoppedVisitor());
        }

        private static int GetNumberPushedByInstruction(Instruction current)
        {
            return current.Accept(new NumberPushedVisitor());
        }

        private bool IncludeInstructionsUntilStackEmpty(MethodBody body,
            Instruction instruction, BitArray includedInstructions)
        {
            int expectedStackSize = 1;
            int instructionIndex = body.Instructions.IndexOf(instruction);

            // Instruction currently points to the instruction where
            // we have an expression on the stack. We now walk the instructions in
            // reverse to the point where we have nothing on the stack. We can then
            // replay the instructions from there to get the expression.
            for (int index = instructionIndex - 1; index >= 0; index--)
            {
                Instruction current = body.Instructions[index];
                includedInstructions[index] = true;

                int popped = GetNumberPoppedByInstruction(current);
                int pushed = GetNumberPushedByInstruction(current);

                expectedStackSize += popped - pushed;

                if (expectedStackSize == 0)
                {
                    // We might be left with a set of instructions where the first usage of
                    // a variable is a read, e.g. a ldloc.2. In these cases, we need to include
                    // the instructions that initialize the variable so that the first usage
                    // of a variable is a write instead.
                    IncludeVariableInitializationInstructions(body, includedInstructions);

                    return true;
                }
            }

            return false;
        }

        private void IncludeVariableInitializationInstructions(
            MethodBody body,
            BitArray includedInstructions)
        {
            while (true)
            {
                List<Instruction> instructions = body.Instructions
                    .Where((t, i) => includedInstructions[i])
                    .ToList();

                VariableUsage firstReadUsage = _instructionHelper
                    .GetUsages(instructions)
                    .GroupBy(usage => usage.Index)
                    .Select(group => group.First())
                    .FirstOrDefault(usage => usage.Operation == VariableOperation.Read);

                if (firstReadUsage == null)
                {
                    return;
                }

                IncludeWriteOfVariable(body, includedInstructions, firstReadUsage);
            }
        }

        private void IncludeWriteOfVariable(MethodBody body, BitArray includedInstructions, VariableUsage readUsage)
        {
            int readIndex = body.Instructions.IndexOf(readUsage.Instruction);
            List<Instruction> precedingInstructions = body.Instructions.Take(readIndex).ToList();

            VariableUsage writeUsage = _instructionHelper
                .GetUsages(precedingInstructions)
                .LastOrDefault(usage => usage.Operation == VariableOperation.Write && usage.Index == readUsage.Index);

            if (writeUsage == null)
            {
                throw new InvalidOperationException("Could not find write of variable " + readUsage.Index);
            }

            int writeIndex = body.Instructions.IndexOf(writeUsage.Instruction);

            includedInstructions[writeIndex] = true;
            IncludeInstructionsUntilStackEmpty(body, writeUsage.Instruction, includedInstructions);
        }
    }
}