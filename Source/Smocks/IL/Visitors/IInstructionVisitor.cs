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
using Smocks.Utility;

namespace Smocks.IL.Visitors
{
    internal interface IInstructionVisitor : IInstructionVisitor<Unit>
    {
    }

    internal interface IInstructionVisitor<out T>
    {
        T Visit(Instruction instruction);

        T VisitInlineArg(Instruction instruction, ParameterReference operand);

        T VisitInlineBrTarget(Instruction instruction, Instruction operand);

        T VisitInlineField(Instruction instruction, FieldReference operand);

        T VisitInlineInteger(Instruction instruction, long operand);

        T VisitInlineInteger(Instruction instruction, int operand);

        T VisitInlineInteger(Instruction instruction, sbyte operand);

        T VisitInlineMethod(Instruction instruction, MethodReference operand);

        T VisitInlineR(Instruction instruction, float operand);

        T VisitInlineR8(Instruction instruction, double operand);

        T VisitInlineSig(Instruction instruction, CallSite operand);

        T VisitInlineString(Instruction instruction, string operand);

        T VisitInlineSwitch(Instruction instruction, Instruction[] operand);

        T VisitInlineTok(Instruction instruction, TypeReference operand);

        T VisitInlineTok(Instruction instruction, FieldReference operand);

        T VisitInlineTok(Instruction instruction, MethodReference operand);

        T VisitInlineType(Instruction instruction, TypeReference operand);

        T VisitInlineVar(Instruction instruction, VariableReference operand);
    }
}