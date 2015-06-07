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
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.IL.Visitors;

namespace Smocks.IL
{
    internal static class InstructionExtensions
    {
        internal static T Accept<T>(this Instruction instruction, IInstructionVisitor<T> visitor)
        {
            switch (instruction.OpCode.OperandType)
            {
                case OperandType.InlineField:
                    return visitor.VisitInlineField(instruction, (FieldReference)instruction.Operand);

                case OperandType.ShortInlineI:
                    return visitor.VisitInlineInteger(instruction, (sbyte)instruction.Operand);

                case OperandType.InlineI:
                    return visitor.VisitInlineInteger(instruction, (int)instruction.Operand);

                case OperandType.InlineString:
                    return visitor.VisitInlineString(instruction, (string)instruction.Operand);

                case OperandType.ShortInlineVar:
                case OperandType.InlineVar:
                    return visitor.VisitInlineVar(instruction, (VariableReference)instruction.Operand);

                case OperandType.InlineMethod:
                    return visitor.VisitInlineMethod(instruction, (MethodReference)instruction.Operand);

                case OperandType.InlineTok:
                    return VisitInlineTok(instruction, visitor);

                case OperandType.InlineType:
                    return visitor.VisitInlineType(instruction, (TypeReference)instruction.Operand);

                case OperandType.InlineBrTarget:
                case OperandType.ShortInlineBrTarget:
                    return visitor.VisitInlineBrTarget(instruction, (Instruction)instruction.Operand);

                case OperandType.InlineI8:
                    return visitor.VisitInlineInteger(instruction, (long)instruction.Operand);

                case OperandType.InlineR:
                    return visitor.VisitInlineR8(instruction, (double)instruction.Operand);

                case OperandType.ShortInlineR:
                    return visitor.VisitInlineR(instruction, (float)instruction.Operand);

                case OperandType.InlineSwitch:
                    return visitor.VisitInlineSwitch(instruction, (Instruction[])instruction.Operand);

                case OperandType.InlineArg:
                case OperandType.ShortInlineArg:
                    return visitor.VisitInlineArg(instruction, (ParameterDefinition)instruction.Operand);

                case OperandType.InlineSig:
                    return visitor.VisitInlineSig(instruction, (CallSite)instruction.Operand);
            }

            if (instruction.Operand != null)
            {
                throw new NotSupportedException("Unknown operand type");
            }

            return visitor.Visit(instruction);
        }

        private static T VisitInlineTok<T>(Instruction instruction, IInstructionVisitor<T> visitor)
        {
            MethodReference methodReference = instruction.Operand as MethodReference;
            if (methodReference != null)
            {
                return visitor.VisitInlineTok(instruction, methodReference);
            }

            FieldReference fieldReference = instruction.Operand as FieldReference;
            if (fieldReference != null)
            {
                return visitor.VisitInlineTok(instruction, fieldReference);
            }

            TypeReference typeReference = instruction.Operand as TypeReference;
            if (typeReference != null)
            {
                return visitor.VisitInlineTok(instruction, typeReference);
            }

            throw new NotSupportedException("Unknown token type");
        }
    }
}