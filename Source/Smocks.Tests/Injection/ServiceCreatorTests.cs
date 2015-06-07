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
using System.Text;
using Moq;
using NUnit.Framework;
using Smocks.Injection;
using Smocks.Setups;

namespace Smocks.Tests.Injection
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ServiceCreatorTestsB
    {
        private Mock<IServiceLocatorContainer> _containerMock;

        [TestCase]
        public void Create_ArgumentsCannotBeResolved_ThrowsException()
        {
            var subject = new ServiceCreator();
            Assert.Throws<InvalidOperationException>(() =>
            {
                subject.Create(typeof(SetupMatcher), _containerMock.Object);
            });
        }

        [TestCase]
        public void Create_Interface_ThrowsException()
        {
            var subject = new ServiceCreator();
            Assert.Throws<InvalidOperationException>(() =>
            {
                subject.Create(typeof(IDisposable), _containerMock.Object);
            });
        }

        [TestCase("Hello, Smocks!")]
        public void Create_MultipleValidConstructors_SelectsConstructorWithMostParameters(string value)
        {
            object instance = value;
            _containerMock
                .Setup(container => container.TryResolve(typeof(string), out instance))
                .Returns(true);

            var subject = new ServiceCreator();
            TestClass result = (TestClass)subject.Create(typeof(TestClass), _containerMock.Object);

            Assert.AreEqual(value, result.First);
            Assert.AreEqual(value, result.Second);
        }

        [TestCase]
        public void Create_TypeWithDefaultConstructor_ReturnsNewInstance()
        {
            var subject = new ServiceCreator();
            object result = subject.Create(typeof(StringBuilder), _containerMock.Object);

            Assert.NotNull(result);
            Assert.IsInstanceOf<StringBuilder>(result);
        }

        [SetUp]
        public void Setup()
        {
            _containerMock = new Mock<IServiceLocatorContainer>();

            object instance;
            _containerMock
                .Setup(container => container.TryResolve(It.IsAny<Type>(), out instance))
                .Returns(false);
        }

        public class TestClass
        {
            public TestClass(string first)
            {
                First = first;
            }

            public TestClass(string first, string second)
            {
                First = first;
                Second = second;
            }

            public string First { get; set; }

            public string Second { get; set; }
        }
    }
}