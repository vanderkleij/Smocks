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
using Mono.Cecil;
using Mono.Cecil.Cil;
using Moq;
using NUnit.Framework;
using Smocks.IL;
using Smocks.IL.Visitors;
using Smocks.Tests.TestUtility;
using Smocks.Utility;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class InstructionExtensionsTests
    {
        private Mock<IInstructionVisitor<Unit>> _visitorMock;

        [TestCase]
        public void Accept_InlineArgOperand_InvokesVisitor()
        {
            var parameter = TestDataFactory.CreateParameterDefinition(256);

            var instruction = Instruction.Create(OpCodes.Ldarg, parameter);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineArg(instruction, parameter))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineBrTargetOperand_InvokesVisitor()
        {
            Instruction other = Instruction.Create(OpCodes.Nop);
            var instruction = Instruction.Create(OpCodes.Br, other);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineBrTarget(instruction, other))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineFieldOperand_InvokesVisitor()
        {
            var instruction = TestDataFactory.CreateInlineFieldInstruction();
            _visitorMock
                .Setup(visitor => visitor.VisitInlineField(instruction, (FieldReference)instruction.Operand))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineFieldTokenOperand_InvokesVisitor()
        {
            FieldReference field = CecilUtility.Import(ReflectionUtility.GetField(() => OpCodes.Nop));
            var instruction = Instruction.Create(OpCodes.Ldtoken, field);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineTok(instruction, field))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase(5464644564545)]
        public void Accept_InlineI8Operand_InvokesVisitor(long value)
        {
            var instruction = Instruction.Create(OpCodes.Ldc_I8, value);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineInteger(instruction, value))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase(546464456)]
        public void Accept_InlineIOperand_InvokesVisitor(int value)
        {
            var instruction = Instruction.Create(OpCodes.Ldc_I4, value);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineInteger(instruction, value))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineMethodOperand_InvokesVisitor()
        {
            var method = CecilUtility.Import(ReflectionUtility.GetMethod(() => Console.WriteLine()));
            var instruction = Instruction.Create(OpCodes.Call, method);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineMethod(instruction, method))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineMethodTokenOperand_InvokesVisitor()
        {
            var method = CecilUtility.Import(ReflectionUtility.GetMethod(() => Console.WriteLine()));
            var instruction = Instruction.Create(OpCodes.Ldtoken, method);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineTok(instruction, method))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase(5464644564545.0)]
        public void Accept_InlineROperand_InvokesVisitor(double value)
        {
            var instruction = Instruction.Create(OpCodes.Ldc_R8, value);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineR8(instruction, value))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineSigOperand_InvokesVisitor()
        {
            var callsite = new CallSite(CecilUtility.Import(typeof(object)));

            var instruction = Instruction.Create(OpCodes.Calli, callsite);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineSig(instruction, callsite))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase("Test")]
        public void Accept_InlineStringOperand_InvokesVisitor(string value)
        {
            var instruction = Instruction.Create(OpCodes.Ldstr, value);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineString(instruction, value))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineTypeOperand_InvokesVisitor()
        {
            TypeReference type = CecilUtility.Import(typeof(object));
            var instruction = Instruction.Create(OpCodes.Initobj, type);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineType(instruction, type))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineTypeTokenOperand_InvokesVisitor()
        {
            TypeReference type = CecilUtility.Import(typeof(object));
            var instruction = Instruction.Create(OpCodes.Ldtoken, type);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineTok(instruction, type))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_InlineVarOperand_InvokesVisitor()
        {
            var variable = new VariableDefinition(CecilUtility.Import(typeof(object)));
            var instruction = Instruction.Create(OpCodes.Ldloc, variable);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineVar(instruction, variable))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_NoOperand_InvokesVisitor()
        {
            var instruction = Instruction.Create(OpCodes.Nop);
            _visitorMock.Setup(visitor => visitor.Visit(instruction))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_ShortInlineArgOperand_InvokesVisitor()
        {
            var parameter = TestDataFactory.CreateParameterDefinition(1);

            var instruction = Instruction.Create(OpCodes.Ldarg_S, parameter);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineArg(instruction, parameter))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_ShortInlineBrTargetOperand_InvokesVisitor()
        {
            Instruction other = Instruction.Create(OpCodes.Nop);
            var instruction = Instruction.Create(OpCodes.Br_S, other);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineBrTarget(instruction, other))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase((sbyte)-50)]
        public void Accept_ShortInlineIOperand_InvokesVisitor(sbyte value)
        {
            var instruction = Instruction.Create(OpCodes.Ldc_I4_S, value);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineInteger(instruction, value))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase(5464644564545.0f)]
        public void Accept_ShortInlineROperand_InvokesVisitor(float value)
        {
            var instruction = Instruction.Create(OpCodes.Ldc_R4, value);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineR(instruction, value))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_ShortInlineSwitchOperand_InvokesVisitor()
        {
            Instruction[] others = new[]
            {
                Instruction.Create(OpCodes.Nop),
                Instruction.Create(OpCodes.Nop),
            };

            var instruction = Instruction.Create(OpCodes.Switch, others);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineSwitch(instruction, others))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [TestCase]
        public void Accept_ShortInlineVarOperand_InvokesVisitor()
        {
            var variable = new VariableDefinition(CecilUtility.Import(typeof(object)));
            var instruction = Instruction.Create(OpCodes.Ldloc_S, variable);
            _visitorMock
                .Setup(visitor => visitor.VisitInlineVar(instruction, variable))
                .Returns(Unit.Value)
                .Verifiable();

            instruction.Accept(_visitorMock.Object);
            _visitorMock.Verify();
        }

        [SetUp]
        public void Setup()
        {
            _visitorMock = new Mock<IInstructionVisitor<Unit>>(MockBehavior.Strict);
        }
    }
}