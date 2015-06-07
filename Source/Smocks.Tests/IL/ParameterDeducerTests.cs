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
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Moq;
using NUnit.Framework;
using Smocks.IL;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ParameterDeducerTests
    {
        private Mock<IInstructionHelper> _instructionHelperMock;

        [TestCase]
        public void GetParameters_EmptyInstructions_EmptyResult()
        {
            MethodReference method = CecilUtility.Import(() => Console.WriteLine());

            var subject = new ParameterDeducer(_instructionHelperMock.Object);
            var result = subject.GetParameters(method, Enumerable.Empty<Instruction>());

            Assert.AreEqual(0, result.Length);
        }

        [TestCase]
        public void GetParameters_InstructionsThatReadParameters_ReturnsParameters()
        {
            MethodReference method = CecilUtility.Import(() => string.Format("Test", 12));

            var instructions = new[]
            {
                Instruction.Create(OpCodes.Ldarg_1),
                Instruction.Create(OpCodes.Ldarg_0)
            };

            Parameter parameter = new Parameter(CecilUtility.Import(typeof(string)), 0);
            _instructionHelperMock
                .Setup(helper => helper.ReadsParameter(method, instructions[0], out parameter))
                .Returns(true);

            Parameter parameter2 = new Parameter(CecilUtility.Import(typeof(int)), 1);
            _instructionHelperMock
                .Setup(helper => helper.ReadsParameter(method, instructions[1], out parameter2))
                .Returns(true);

            var subject = new ParameterDeducer(_instructionHelperMock.Object);
            TypeReference[] result = subject.GetParameters(method, instructions);

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(parameter.ParameterType, result[0]);
            Assert.AreEqual(parameter2.ParameterType, result[1]);
        }

        [SetUp]
        public void Setup()
        {
            _instructionHelperMock = new Mock<IInstructionHelper>(MockBehavior.Strict);
        }
    }
}