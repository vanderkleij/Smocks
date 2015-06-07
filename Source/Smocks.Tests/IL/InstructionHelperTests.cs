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
        private static readonly MethodReference StaticMethodReference =
            CecilUtility.Import(ReflectionUtility.GetMethod(() => 
                StaticMethod(0, "One", 2, 3, 4, 5, 6)));

        private static readonly MethodReference InstanceMethodReference =
            CecilUtility.Import(ReflectionUtility.GetMethod(() =>
                new InstructionHelperTests().InstanceMethod(0, "One", 2, 3, 4, 5, 6)));

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

        public static void StaticMethod(int arg0, string arg1, float arg2, 
            double arg3, short arg4, long arg5, byte arg6)
        {
        }

        public void InstanceMethod(int arg0, string arg1, float arg2,
            double arg3, short arg4, long arg5, byte arg6)
        {
        }
    }
}