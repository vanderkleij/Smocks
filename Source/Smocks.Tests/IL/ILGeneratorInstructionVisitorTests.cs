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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Moq;
using NUnit.Framework;
using Smocks.IL;
using Smocks.Tests.TestUtility;
using OpCodes = Mono.Cecil.Cil.OpCodes;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ILGeneratorInstructionVisitorTests
    {
        /// <summary>
        /// The test field used to import for testing purposes.
        /// </summary>
        public string TestField;

        private Mock<IILGenerator> _generatorMock;

        [SetUp]
        public void Setup()
        {
            _generatorMock = new Mock<IILGenerator>(MockBehavior.Strict);
            _generatorMock.Setup(generator => generator.DefineLabel()).Returns(new Label());
            _generatorMock.Setup(generator => generator.MarkLabel(It.IsAny<Label>()));
        }

        [TestCase]
        public void Visit_NoOperand_EmitsInstruction()
        {
            _generatorMock
                .Setup(generator => generator.Emit(System.Reflection.Emit.OpCodes.Nop))
                .Verifiable();
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);
            subject.Visit(Instruction.Create(OpCodes.Nop));

            _generatorMock.Verify();
        }

        [TestCase]
        public void VisitInlineArg_ByteArgument_EmitsInstruction()
        {
            var method = CecilUtility.Import(ReflectionUtility.GetMethod(() => int.Parse(string.Empty))).Resolve();

            ParameterDefinition parameter = method.Parameters[0];

            _generatorMock
                .Setup(generator => generator.Emit(System.Reflection.Emit.OpCodes.Ldarg_S, (byte)parameter.Index))
                .Verifiable();
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            var instruction = Instruction.Create(OpCodes.Ldarg_S, parameter);
            subject.VisitInlineArg(instruction, parameter);

            _generatorMock.Verify();
        }

        [TestCase]
        public void VisitInlineArg_UShortArgument_EmitsInstruction()
        {
            ParameterDefinition parameter = TestDataFactory.CreateParameterDefinition(256);

            _generatorMock
                .Setup(generator => generator.Emit(System.Reflection.Emit.OpCodes.Ldarg, parameter.Index))
                .Verifiable();
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            var instruction = Instruction.Create(OpCodes.Ldarg, parameter);
            subject.VisitInlineArg(instruction, parameter);

            _generatorMock.Verify();
        }

        [TestCase]
        public void VisitInlineBrTarget_EmitsInstruction()
        {
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            _generatorMock
                .Setup(generator => generator.Emit(System.Reflection.Emit.OpCodes.Br, It.IsAny<Label>()))
                .Verifiable();

            var target = Instruction.Create(OpCodes.Nop);
            var instruction = Instruction.Create(OpCodes.Br, target);

            subject.VisitInlineBrTarget(instruction, target);

            _generatorMock.Verify();
        }

        [TestCase]
        public void VisitInlineField_EmitsInstruction()
        {
            TypeDefinition type = CecilUtility.Import(GetType()).Resolve();
            FieldDefinition fieldDefinition = type.Fields.First(f => f.Name == "TestField");

            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            _generatorMock
                .Setup(generator => generator.Emit(
                    System.Reflection.Emit.OpCodes.Ldfld,
                    It.Is<FieldInfo>(field => field.Name == fieldDefinition.Name)))
                .Verifiable();

            var instruction = Instruction.Create(OpCodes.Ldfld, fieldDefinition);

            subject.VisitInlineField(instruction, fieldDefinition);

            _generatorMock.Verify();
        }

        [TestCase(3453453)]
        public void VisitInlineInteger_IntOperand_EmitsInstruction(int value)
        {
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            _generatorMock
                .Setup(generator => generator.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, value))
                .Verifiable();

            var instruction = Instruction.Create(OpCodes.Ldc_I4, value);
            subject.VisitInlineInteger(instruction, value);

            _generatorMock.Verify();
        }

        [TestCase(3453453565656L)]
        public void VisitInlineInteger_IntOperand_EmitsInstruction(long value)
        {
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            _generatorMock
                .Setup(generator => generator.Emit(System.Reflection.Emit.OpCodes.Ldc_I8, value))
                .Verifiable();

            var instruction = Instruction.Create(OpCodes.Ldc_I8, value);
            subject.VisitInlineInteger(instruction, value);

            _generatorMock.Verify();
        }

        [TestCase]
        public void VisitInlineMethod_ConstructorOperand_EmitsInstruction()
        {
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            _generatorMock
                .Setup(generator => generator.Emit(
                    System.Reflection.Emit.OpCodes.Newobj,
                    It.Is<ConstructorInfo>(constructor => constructor.DeclaringType.Name == "MemoryStream")))
                .Verifiable();

            var method = ReflectionUtility.GetMethod(() => new MemoryStream());
            var instruction = Instruction.Create(OpCodes.Newobj, CecilUtility.Import(method));
            subject.VisitInlineMethod(instruction, (MethodReference)instruction.Operand);

            _generatorMock.Verify();
        }

        [TestCase]
        public void VisitInlineSwitch_EmitsInstruction()
        {
            var subject = new ILGeneratorInstructionVisitor(_generatorMock.Object);

            _generatorMock
                .Setup(generator => generator.Emit(
                    System.Reflection.Emit.OpCodes.Switch,
                    It.Is<Label[]>(labels => labels.Length == 2)))
                .Verifiable();

            var targets = new[] { Instruction.Create(OpCodes.Nop), Instruction.Create(OpCodes.Nop) };
            var instruction = Instruction.Create(OpCodes.Switch, targets);

            subject.VisitInlineSwitch(instruction, targets);

            _generatorMock.Verify();
        }
    }
}