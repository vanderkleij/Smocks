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
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using Smocks.IL;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class InstructionHelperTests
    {
        private static readonly MethodReference InstanceMethodReference =
            CecilUtility.Import(ReflectionUtility.GetMethod(() =>
                new InstructionHelperTests().InstanceMethod(0, "One", 2, 3, 4, 5, 6)));

        private static readonly MethodReference StaticMethodReference =
            CecilUtility.Import(ReflectionUtility.GetMethod(() =>
                StaticMethod(0, "One", 2, 3, 4, 5, 6)));

        public static void StaticMethod(int arg0, string arg1, float arg2,
            double arg3, short arg4, long arg5, byte arg6)
        {
        }

        public void InstanceMethod(int arg0, string arg1, float arg2,
            double arg3, short arg4, long arg5, byte arg6)
        {
        }

        [TestCase(4, typeof(double))]
        [TestCase(5, typeof(short))]
        public void ReadsParameter_InstanceMethodLdarg_ReturnsSpecifiedParameter(
            int parameterIndex, Type expectedType)
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            var instruction = Instruction.Create(OpCodes.Ldarg,
                StaticMethodReference.Parameters[parameterIndex - 1]);
            bool result = subject.ReadsParameter(InstanceMethodReference, instruction, out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(parameterIndex, parameter.Index);
            Assert.AreEqual(expectedType.FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_InstanceMethodLdarg0_ReturnsDeclaringType()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(InstanceMethodReference,
                Instruction.Create(OpCodes.Ldarg_0), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(0, parameter.Index);
            Assert.AreEqual(typeof(InstructionHelperTests).FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_InstanceMethodLdarg1_ReturnsFirstParameter()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(InstanceMethodReference,
                Instruction.Create(OpCodes.Ldarg_1), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(1, parameter.Index);
            Assert.AreEqual(typeof(int).FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_InstanceMethodLdarg2_ReturnsSecondParameter()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(InstanceMethodReference,
                Instruction.Create(OpCodes.Ldarg_2), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(2, parameter.Index);
            Assert.AreEqual(typeof(string).FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_InstanceMethodLdarg3_ReturnsThirdParameter()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(InstanceMethodReference,
                Instruction.Create(OpCodes.Ldarg_3), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(3, parameter.Index);
            Assert.AreEqual(typeof(float).FullName, parameter.ParameterType.FullName);
        }

        [TestCase(4, typeof(double))]
        [TestCase(5, typeof(short))]
        public void ReadsParameter_InstanceMethodLdargS_ReturnsSpecifiedParameter(
            int parameterIndex, Type expectedType)
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            var instruction = Instruction.Create(OpCodes.Ldarg_S,
                StaticMethodReference.Parameters[parameterIndex - 1]);
            bool result = subject.ReadsParameter(InstanceMethodReference, instruction, out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(parameterIndex, parameter.Index);
            Assert.AreEqual(expectedType.FullName, parameter.ParameterType.FullName);
        }

        [TestCase(4, typeof(short))]
        [TestCase(5, typeof(long))]
        public void ReadsParameter_StaticMethodLdarg_ReturnsSpecifiedParameter(
            int parameterIndex, Type expectedType)
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            var instruction = Instruction.Create(OpCodes.Ldarg,
                StaticMethodReference.Parameters[parameterIndex]);
            bool result = subject.ReadsParameter(StaticMethodReference, instruction, out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(parameterIndex, parameter.Index);
            Assert.AreEqual(expectedType.FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_StaticMethodLdarg0_ReturnsFirstParameter()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(StaticMethodReference,
                Instruction.Create(OpCodes.Ldarg_0), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(0, parameter.Index);
            Assert.AreEqual(typeof(int).FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_StaticMethodLdarg1_ReturnsSecondParameter()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(StaticMethodReference,
                Instruction.Create(OpCodes.Ldarg_1), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(1, parameter.Index);
            Assert.AreEqual(typeof(string).FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_StaticMethodLdarg2_ReturnsThirdParameter()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(StaticMethodReference,
                Instruction.Create(OpCodes.Ldarg_2), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(2, parameter.Index);
            Assert.AreEqual(typeof(float).FullName, parameter.ParameterType.FullName);
        }

        [TestCase]
        public void ReadsParameter_StaticMethodLdarg3_ReturnsFourthParameter()
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            bool result = subject.ReadsParameter(StaticMethodReference,
                Instruction.Create(OpCodes.Ldarg_3), out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(3, parameter.Index);
            Assert.AreEqual(typeof(double).FullName, parameter.ParameterType.FullName);
        }

        [TestCase(4, typeof(short))]
        [TestCase(5, typeof(long))]
        public void ReadsParameter_StaticMethodLdargS_ReturnsSpecifiedParameter(
            int parameterIndex, Type expectedType)
        {
            var subject = new InstructionHelper();

            Parameter parameter;
            var instruction = Instruction.Create(OpCodes.Ldarg_S,
                StaticMethodReference.Parameters[parameterIndex]);
            bool result = subject.ReadsParameter(StaticMethodReference, instruction, out parameter);

            Assert.IsTrue(result);
            Assert.AreEqual(parameterIndex, parameter.Index);
            Assert.AreEqual(expectedType.FullName, parameter.ParameterType.FullName);
        }
    }
}