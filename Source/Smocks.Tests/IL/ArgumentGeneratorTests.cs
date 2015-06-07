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
using System.Globalization;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Smocks.IL;
using Smocks.Injection;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ArgumentGeneratorTests
    {
        private Mock<IServiceLocatorContainer> _containerMock;
        private Mock<IServiceCreator> _serviceCreatorMock;
        private Mock<IServiceLocator> _serviceLocatorMock;

        [TestCase]
        public void Constructor_ServiceCreatorNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ArgumentGenerator(_serviceLocatorMock.Object, null));

            Assert.AreEqual("serviceCreator", exception.ParamName);
        }

        [TestCase]
        public void Constructor_ServiceLocatorNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ArgumentGenerator(null, _serviceCreatorMock.Object));

            Assert.AreEqual("serviceLocator", exception.ParamName);
        }

        [TestCase]
        public void GetArguments_MultipleParameters_InvokesServiceCreator()
        {
            var subject = new ArgumentGenerator(_serviceLocatorMock.Object, _serviceCreatorMock.Object);
            Type[] types = { typeof(IDisposable), typeof(IFormatProvider) };

            var created1 = new MemoryStream();
            var created2 = CultureInfo.CurrentCulture;

            _serviceCreatorMock
                .Setup(creator => creator.Create(types[0], _containerMock.Object))
                .Returns(created1);

            _serviceCreatorMock
                .Setup(creator => creator.Create(types[1], _containerMock.Object))
                .Returns(created2);

            var result = subject.GetArguments(types, null).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreSame(created1, result[0]);
            Assert.AreSame(created2, result[1]);
        }

        [TestCase]
        public void GetArguments_OneParameterThatDoesntMatchTarget_ThrowsException()
        {
            var subject = new ArgumentGenerator(_serviceLocatorMock.Object, _serviceCreatorMock.Object);

            Assert.Throws<ArgumentException>(() =>
            {
                subject.GetArguments(new[] { typeof(IDisposable) }, "Test").ToList();
            });
        }

        [TestCase]
        public void GetArguments_OneParameterThatMatchesTarget_ReturnsTarget()
        {
            var subject = new ArgumentGenerator(_serviceLocatorMock.Object, _serviceCreatorMock.Object);

            var target = new MemoryStream();
            var result = subject.GetArguments(new[] { typeof(IDisposable) }, target).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(target, result[0]);
        }

        [TestCase]
        public void GetArguments_ZeroParameters_ReturnsEmptyArray()
        {
            var subject = new ArgumentGenerator(_serviceLocatorMock.Object, _serviceCreatorMock.Object);
            Assert.IsEmpty(subject.GetArguments(new Type[0], null));
        }

        [SetUp]
        public void Setup()
        {
            _containerMock = new Mock<IServiceLocatorContainer>(MockBehavior.Strict);

            _serviceCreatorMock = new Mock<IServiceCreator>(MockBehavior.Strict);

            _serviceLocatorMock = new Mock<IServiceLocator>(MockBehavior.Strict);
            _serviceLocatorMock.SetupGet(locator => locator.Container).Returns(_containerMock.Object);
        }
    }
}