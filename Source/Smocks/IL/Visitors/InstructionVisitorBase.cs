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

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Smocks.IL.Visitors
{
    internal abstract class InstructionVisitorBase<T> : IInstructionVisitor<T>
    {
        public abstract T Visit(Instruction instruction);

        public virtual T VisitField(Instruction instruction, FieldReference operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitInlineArg(Instruction instruction, ParameterReference operand)
        {
            return VisitParameter(instruction, operand);
        }

        public T VisitInlineBrTarget(Instruction instruction, Instruction operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitInlineField(Instruction instruction, FieldReference operand)
        {
            return VisitField(instruction, operand);
        }

        public T VisitInlineInteger(Instruction instruction, long operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitInlineInteger(Instruction instruction, int operand)
        {
            return Visit(instruction);
        }

        public T VisitInlineInteger(Instruction instruction, sbyte operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitInlineMethod(Instruction instruction, MethodReference operand)
        {
            return VisitMethod(instruction, operand);
        }

        public T VisitInlineR(Instruction instruction, float operand)
        {
            return Visit(instruction);
        }

        public T VisitInlineR8(Instruction instruction, double operand)
        {
            return Visit(instruction);
        }

        public T VisitInlineSig(Instruction instruction, CallSite operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitInlineString(Instruction instruction, string operand)
        {
            return Visit(instruction);
        }

        public T VisitInlineSwitch(Instruction instruction, Instruction[] operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitInlineTok(Instruction instruction, TypeReference typeReference)
        {
            return VisitType(instruction, typeReference);
        }

        public virtual T VisitInlineTok(Instruction instruction, FieldReference fieldReference)
        {
            return VisitField(instruction, fieldReference);
        }

        public virtual T VisitInlineTok(Instruction instruction, MethodReference methodReference)
        {
            return VisitMethod(instruction, methodReference);
        }

        public virtual T VisitInlineType(Instruction instruction, TypeReference operand)
        {
            return VisitType(instruction, operand);
        }

        public virtual T VisitInlineVar(Instruction instruction, VariableReference operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitMethod(Instruction instruction, MethodReference operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitParameter(Instruction instruction, ParameterReference operand)
        {
            return Visit(instruction);
        }

        public virtual T VisitType(Instruction instruction, TypeReference typeReference)
        {
            return Visit(instruction);
        }
    }
}