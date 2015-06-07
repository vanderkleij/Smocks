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
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using NUnit.Framework;
using Smocks.IL;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ILGeneratorWrapperTests
    {
        [TestCase((byte)12)]
        public void DeclareLocal_EmitsProvidedInstructions(byte value)
        {
            var method = new DynamicMethod("Test", typeof(byte), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            var local = subject.DeclareLocal(typeof(byte), false);
            subject.Emit(OpCodes.Ldc_I4_S, value);
            subject.Emit(OpCodes.Stloc, local);
            subject.Emit(OpCodes.Ldloc, local);
            subject.Emit(OpCodes.Ret);
            byte result = (byte)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }

        [TestCase(42)]
        public void EmitBranch_EmitsProvidedInstructions(int value)
        {
            var method = new DynamicMethod("Test", typeof(int), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            var label = subject.DefineLabel();
            subject.Emit(OpCodes.Ldc_I4, value);
            subject.Emit(OpCodes.Br, label);
            subject.Emit(OpCodes.Ldc_I4, 123);
            subject.MarkLabel(label);
            subject.Emit(OpCodes.Ret);
            int result = (int)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }

        [TestCase(1.23)]
        public void EmitDouble_EmitsProvidedInstructions(double value)
        {
            var method = new DynamicMethod("Test", typeof(double), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            subject.Emit(OpCodes.Ldc_R8, value);
            subject.Emit(OpCodes.Ret);
            double result = (double)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }

        [TestCase(1.23f)]
        public void EmitFloat_EmitsProvidedInstructions(float value)
        {
            var method = new DynamicMethod("Test", typeof(float), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            subject.Emit(OpCodes.Ldc_R4, value);
            subject.Emit(OpCodes.Ret);
            float result = (float)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }

        [TestCase(42)]
        public void EmitInt_EmitsProvidedInstructions(int value)
        {
            var method = new DynamicMethod("Test", typeof(int), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            subject.Emit(OpCodes.Ldc_I4, value);
            subject.Emit(OpCodes.Ret);
            int result = (int)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }

        [TestCase((byte)12)]
        public void EmitLdloc_EmitsProvidedInstructions(byte value)
        {
            var method = new DynamicMethod("Test", typeof(byte), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            var local = subject.DeclareLocal(typeof(byte), false);
            subject.Emit(OpCodes.Ldc_I4_S, value);
            subject.Emit(OpCodes.Stloc, local);
            subject.Emit(OpCodes.Ldloc, (short)0);
            subject.Emit(OpCodes.Ret);
            byte result = (byte)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }

        [TestCase(42L)]
        public void EmitLong_EmitsProvidedInstructions(long value)
        {
            var method = new DynamicMethod("Test", typeof(long), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            subject.Emit(OpCodes.Ldc_I8, value);
            subject.Emit(OpCodes.Ret);
            long result = (long)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }

        [TestCase("Hello, world")]
        public void EmitString_EmitsProvidedInstructions(string message)
        {
            var method = new DynamicMethod("Test", typeof(string), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            subject.Emit(OpCodes.Ldstr, message);
            subject.Emit(OpCodes.Ret);
            string result = (string)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(message, result);
        }

        [TestCase(42)]
        public void EmitSwitch_EmitsProvidedInstructions(int value)
        {
            var method = new DynamicMethod("Test", typeof(int), new Type[0]);
            var generator = method.GetILGenerator();

            var subject = new ILGeneratorWrapper(generator);

            // Act
            var endOfMethod = subject.DefineLabel();
            var labels = new[] { subject.DefineLabel(), subject.DefineLabel() };

            subject.Emit(OpCodes.Ldc_I4_0);
            subject.Emit(OpCodes.Switch, labels);
            subject.MarkLabel(labels[0]);
            subject.Emit(OpCodes.Ldc_I4, value);
            subject.Emit(OpCodes.Br_S, endOfMethod);
            subject.MarkLabel(labels[1]);
            subject.Emit(OpCodes.Ldc_I4_8, value);
            subject.Emit(OpCodes.Br_S, endOfMethod);
            subject.MarkLabel(endOfMethod);
            subject.Emit(OpCodes.Ret);

            int result = (int)method.Invoke(null, new object[0]);

            // Assert
            Assert.AreEqual(value, result);
        }
    }
}