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

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Smocks.IL
{
    internal class InstructionHelper : IInstructionHelper
    {
        public bool ReadsParameter(MethodReference method, Instruction instruction,
            out Parameter parameter)
        {
            var parameters = method.Parameters;
            int offset = method.HasThis ? 1 : 0;

            TypeReference parameterType = null;
            int index = -1;

            switch (instruction.OpCode.Code)
            {
                case Code.Ldarg_0:
                    parameterType = method.HasThis ? method.DeclaringType : parameters[0].ParameterType;
                    index = 0;
                    break;

                case Code.Ldarg_1:
                    parameterType = parameters[1 - offset].ParameterType;
                    index = 1;
                    break;

                case Code.Ldarg_2:
                    parameterType = parameters[2 - offset].ParameterType;
                    index = 2;
                    break;

                case Code.Ldarg_3:
                    parameterType = parameters[3 - offset].ParameterType;
                    index = 3;
                    break;

                case Code.Ldarg_S:
                case Code.Ldarg:
                case Code.Ldarga:
                case Code.Ldarga_S:
                    parameterType = ((ParameterReference)instruction.Operand).ParameterType;
                    index = ((ParameterReference)instruction.Operand).Index + offset;
                    break;
            }

            parameter = parameterType != null ? new Parameter(parameterType, index) : null;
            return parameter != null;
        }

        public bool TryGetCall(Instruction instruction, out MethodReference calledMethod)
        {
            calledMethod = null;

            if (instruction.OpCode == OpCodes.Callvirt ||
                instruction.OpCode == OpCodes.Call ||
                instruction.OpCode == OpCodes.Newobj)
            {
                calledMethod = (MethodReference)instruction.Operand;
                return true;
            }

            return false;
        }
    }
}