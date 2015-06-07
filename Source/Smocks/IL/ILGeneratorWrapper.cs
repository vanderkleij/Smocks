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
using System.Reflection;
using System.Reflection.Emit;
using Smocks.Utility;

namespace Smocks.IL
{
    internal class ILGeneratorWrapper : IILGenerator
    {
        private readonly ILGenerator _generator;

        internal ILGeneratorWrapper(ILGenerator generator)
        {
            ArgumentChecker.NotNull(generator, () => generator);

            _generator = generator;
        }

        public virtual LocalBuilder DeclareLocal(Type localType)
        {
            return _generator.DeclareLocal(localType);
        }

        public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            return _generator.DeclareLocal(localType, pinned);
        }

        public virtual Label DefineLabel()
        {
            return _generator.DefineLabel();
        }

        public virtual void Emit(OpCode opcode)
        {
            _generator.Emit(opcode);
        }

        public virtual void Emit(OpCode opcode, byte arg)
        {
            _generator.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, ConstructorInfo con)
        {
            _generator.Emit(opcode, con);
        }

        public virtual void Emit(OpCode opcode, double arg)
        {
            _generator.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, FieldInfo field)
        {
            _generator.Emit(opcode, field);
        }

        public virtual void Emit(OpCode opcode, float arg)
        {
            _generator.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, int arg)
        {
            _generator.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, Label label)
        {
            _generator.Emit(opcode, label);
        }

        public virtual void Emit(OpCode opcode, Label[] labels)
        {
            _generator.Emit(opcode, labels);
        }

        public virtual void Emit(OpCode opcode, LocalBuilder local)
        {
            _generator.Emit(opcode, local);
        }

        public virtual void Emit(OpCode opcode, long arg)
        {
            _generator.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, MethodInfo meth)
        {
            _generator.Emit(opcode, meth);
        }

        public void Emit(OpCode opcode, sbyte arg)
        {
            _generator.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, short arg)
        {
            _generator.Emit(opcode, arg);
        }

        public virtual void Emit(OpCode opcode, string str)
        {
            _generator.Emit(opcode, str);
        }

        public virtual void Emit(OpCode opcode, Type cls)
        {
            _generator.Emit(opcode, cls);
        }

        public void MarkLabel(Label label)
        {
            _generator.MarkLabel(label);
        }
    }
}