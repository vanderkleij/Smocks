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

namespace Smocks.IL.Visitors
{
    internal class NumberPoppedVisitor : InstructionVisitorBase<int>
    {
        public override int Visit(Instruction instruction)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Ldc_R4:
                case Code.Ldc_R8:
                case Code.Ldc_I4:
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                case Code.Ldc_I4_M1:
                case Code.Ldc_I4_S:
                case Code.Ldtoken:
                case Code.Ldarg:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldnull:
                case Code.Ldstr:
                case Code.Ldloc:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc_S:
                case Code.Ldloca:
                case Code.Ldloca_S:
                case Code.Newobj:
                case Code.Nop:
                case Code.Br:
                    return 0;

                case Code.Newarr:
                case Code.Castclass:
                case Code.Stloc:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Stloc_S:
                case Code.Box:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Dup:
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Pop:
                case Code.Ret:
                    return 1;

                case Code.Stfld:
                case Code.Ceq:
                    return 2;

                case Code.Stelem_Ref:
                    return 3;

                case Code.Call:
                case Code.Calli:
                case Code.Callvirt:
                    return VisitInlineMethod(instruction, (MethodReference)instruction.Operand);
            }

            throw new NotImplementedException(string.Format("Unexpected opcode: {0}", instruction.OpCode.Code));
        }

        public override int VisitInlineMethod(Instruction instruction, MethodReference operand)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Call:
                case Code.Callvirt:
                    return operand.Parameters.Count + (operand.HasThis ? 1 : 0);

                case Code.Newobj:
                    return operand.Parameters.Count;

                default:
                    throw new NotSupportedException("Unexpected opcode");
            }
        }
    }
}