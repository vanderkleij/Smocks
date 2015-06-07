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
using Mono.Cecil.Cil;
using OpCode = System.Reflection.Emit.OpCode;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace Smocks.IL
{
    internal class OpCodeMapper : IOpCodeMapper
    {
        private static readonly Dictionary<Code, OpCode> Lookup = BuildLookup();

        public OpCode Map(Mono.Cecil.Cil.OpCode opCode)
        {
            return Lookup[opCode.Code];
        }

        private static Dictionary<Code, OpCode> BuildLookup()
        {
            var fields = typeof(OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields
                .Where(field => field.FieldType == typeof(OpCode) && GetCode(field.Name).HasValue)
                .ToDictionary(field => GetCode(field.Name).Value, field => (OpCode)field.GetValue(null));
        }

        private static Code? GetCode(string name)
        {
            Code code;
            return Enum.TryParse(name, true, out code) ? code : default(Code?);
        }
    }
}