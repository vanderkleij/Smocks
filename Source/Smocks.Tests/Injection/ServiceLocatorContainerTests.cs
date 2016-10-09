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
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Smocks.Injection;

namespace Smocks.Tests.Injection
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ServiceLocatorContainerTests
    {
        [TestCase]
        public void Constructor_NullSetup_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ServiceLocatorContainer(default(IServiceLocatorSetup)));

            Assert.AreEqual("setup", exception.ParamName);
        }

        [TestCase]
        public void Constructor_ValidSetup_InvokesSetup()
        {
            var setupMock = new Mock<IServiceLocatorSetup>(MockBehavior.Strict);

            IServiceLocatorContainer calledContainer = null;
            setupMock
                .Setup(setup => setup.Configure(It.IsAny<IServiceLocatorContainer>()))
                .Callback<IServiceLocatorContainer>(container => calledContainer = container)
                .Verifiable();

            // Act
            var subject = new ServiceLocatorContainer(setupMock.Object);

            // Assert
            setupMock.Verify();
            Assert.AreEqual(subject, calledContainer);
        }

        [TestCase]
        public void Resolve_MultipleRegisterations_ReturnsLastRegistration()
        {
            var subject = new ServiceLocatorContainer();
            subject.Register<IDisposable, MemoryStream>();
            subject.Register<IDisposable, StringWriter>();

            IDisposable result = subject.Resolve<IDisposable>();

            Assert.IsTrue(result is StringWriter);
        }

        [TestCase]
        public void Resolve_NonRegisteredInterface_ThrowsException()
        {
            var subject = new ServiceLocatorContainer();

            Assert.Throws<InvalidOperationException>(() =>
            {
                subject.Resolve<IDisposable>();
            });
        }

        [TestCase]
        public void Resolve_NonRegisteredTypeWithDefaultConstructor_ThrowsException()
        {
            var subject = new ServiceLocatorContainer();

            Assert.Throws<InvalidOperationException>(() =>
            {
                subject.Resolve<MemoryStream>();
            });
        }

        [TestCase]
        public void Resolve_RegisteredSingletonType_ReturnsSameInstancePerResolve()
        {
            var subject = new ServiceLocatorContainer();
            subject.RegisterSingleton<IDisposable, MemoryStream>();

            var first = subject.Resolve<IDisposable>() as MemoryStream;
            var second = subject.Resolve<IDisposable>() as MemoryStream;

            Assert.NotNull(first);
            Assert.NotNull(second);
            Assert.AreSame(first, second);
        }

        [TestCase]
        public void Resolve_RegisteredType_CreatesNewInstancePerResolve()
        {
            var subject = new ServiceLocatorContainer();
            subject.Register<IDisposable, MemoryStream>();

            var first = subject.Resolve<IDisposable>() as MemoryStream;
            var second = subject.Resolve<IDisposable>() as MemoryStream;

            Assert.NotNull(first);
            Assert.NotNull(second);
            Assert.AreNotSame(first, second);
        }

        [TestCase]
        public void ResolveAll_MultipleRegisterations_ReturnsInstanceOfEveryRegistration()
        {
            var subject = new ServiceLocatorContainer();
            subject.Register<IDisposable, MemoryStream>();
            subject.Register<IDisposable, StringWriter>();

            var result = subject.ResolveAll<IDisposable>()?.ToList();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(x => x is StringWriter));
            Assert.IsTrue(result.Any(x => x is MemoryStream));
        }

        [TestCase]
        public void ResolveAll_NoRegisteration_ReturnsEmptyEnumerable()
        {
            var subject = new ServiceLocatorContainer();
            var result = subject.ResolveAll<IDisposable>();

            Assert.IsEmpty(result);
        }
    }
}