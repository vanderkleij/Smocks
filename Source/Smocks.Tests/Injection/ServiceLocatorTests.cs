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
using System.IO;
using Moq;
using NUnit.Framework;
using Smocks.Injection;

namespace Smocks.Tests.Injection
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ServiceLocatorTests
    {
        [TestCase]
        public void Constructor_NullContainer_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceLocator(null));
            Assert.AreEqual("container", exception.ParamName);
        }

        [TestCase]
        public void Constructor_RegistersSelfAsIServiceLocator()
        {
            var containerMock = new Mock<IServiceLocatorContainer>(MockBehavior.Strict);

            ServiceLocator registeredInstance = null;

            containerMock
                .Setup(container =>
                    container.RegisterSingleton<IServiceLocator, ServiceLocator>(It.IsAny<ServiceLocator>()))
                .Callback<ServiceLocator>(instance => registeredInstance = instance);

            // Act
            var subject = new ServiceLocator(containerMock.Object);

            // Assert
            Assert.AreEqual(subject, registeredInstance);
        }

        [TestCase]
        public void Resolve_InvokesResolveOnContainerAndReturnsResult()
        {
            var expectedResult = new MemoryStream();

            var containerMock = new Mock<IServiceLocatorContainer>();
            containerMock
                .Setup(container => container.Resolve<IDisposable>())
                .Returns(expectedResult)
                .Verifiable();

            // Act
            var subject = new ServiceLocator(containerMock.Object);
            var result = subject.Resolve<IDisposable>();

            // Assert
            containerMock.Verify();
            Assert.AreEqual(expectedResult, result);
        }
    }
}