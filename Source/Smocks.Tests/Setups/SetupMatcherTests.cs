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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Smocks.Setups;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.Setups
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SetupMatcherTests
    {
        private Mock<IArgumentMatcher> _argumentMatcherMock;
        private Mock<ISetupManager> _setupManagerMock;
        private Mock<ITargetMatcher> _targetMatcherMock;

        [TestCase]
        public void Constructor_ArgumentMatcherNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new SetupMatcher(_setupManagerMock.Object, _targetMatcherMock.Object, null));
            Assert.AreEqual("argumentMatcher", exception.ParamName);
        }

        [TestCase]
        public void Constructor_SetupManagerNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new SetupMatcher(null, _targetMatcherMock.Object, _argumentMatcherMock.Object));
            Assert.AreEqual("setupManager", exception.ParamName);
        }

        [TestCase]
        public void Constructor_TargetMatcherNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new SetupMatcher(_setupManagerMock.Object, null, _argumentMatcherMock.Object));
            Assert.AreEqual("targetMatcher", exception.ParamName);
        }

        [TestCase]
        public void GetBestMatchingSetup_GetsSetupsFromSetupManager()
        {
            var subject = new SetupMatcher(_setupManagerMock.Object,
                _targetMatcherMock.Object, _argumentMatcherMock.Object);

            var method = ReflectionUtility.GetMethod(() => Console.WriteLine());
            var arguments = new object[0];

            _setupManagerMock.Setup(manager => manager.GetSetupsForMethod(method))
                .Returns(new List<IInternalSetup>().AsReadOnly())
                .Verifiable();

            subject.GetBestMatchingSetup(method, arguments);

            // Assert
            _setupManagerMock.Verify();
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        public void GetBestMatchingSetup_InstanceMethodWithArguments_ReturnsOnlyOnTargetAndArgumentsMatch(
            bool targetsMatch, bool argumentsMatch)
        {
            var subject = new SetupMatcher(_setupManagerMock.Object,
                _targetMatcherMock.Object, _argumentMatcherMock.Object);

            var method = ReflectionUtility.GetMethod(() => "Test".Trim('c'));
            var arguments = new object[] { "Test", 'c' }; // First argument is the target

            var setups = TestDataFactory.CreateSetups(() => "Test".Trim('c'));
            _setupManagerMock.Setup(manager => manager.GetSetupsForMethod(method))
                .Returns(setups);

            _targetMatcherMock
                .Setup(matcher => matcher.IsMatch(typeof(string), setups[0].MethodCall.Arguments[0], "Test"))
                .Returns(targetsMatch);

            _argumentMatcherMock
                .Setup(matcher => matcher.IsMatch(
                    It.Is<IEnumerable<Expression>>(enumerable => enumerable.SequenceEqual(setups[0].MethodCall.Arguments.Skip(1))),
                    It.Is<IEnumerable<object>>(enumerable => enumerable.SequenceEqual(arguments.Skip(1)))))
                .Returns(argumentsMatch);

            IInternalSetup result = subject.GetBestMatchingSetup(method, arguments);

            if (targetsMatch && argumentsMatch)
                Assert.AreSame(setups[0], result);
            else
                Assert.IsNull(result);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetBestMatchingSetup_InstanceMethodWithoutArguments_InvokesTargetCheckerAndReturnsOnlyOnMatch(
            bool targetsMatch)
        {
            var subject = new SetupMatcher(_setupManagerMock.Object,
                _targetMatcherMock.Object, _argumentMatcherMock.Object);

            var method = ReflectionUtility.GetMethod(() => "Test".Trim());
            var arguments = new object[] { "Test" }; // First argument is the target

            var setups = TestDataFactory.CreateSetups(() => "Test".Trim());
            _setupManagerMock.Setup(manager => manager.GetSetupsForMethod(method))
                .Returns(setups);

            _targetMatcherMock
                .Setup(matcher => matcher.IsMatch(typeof(string), setups[0].MethodCall.Arguments[0], "Test"))
                .Returns(targetsMatch);

            IInternalSetup result = subject.GetBestMatchingSetup(method, arguments);

            if (targetsMatch)
                Assert.AreSame(setups[0], result);
            else
                Assert.IsNull(result);
        }

        [TestCase]
        public void GetBestMatchingSetup_MultipleMatchingSetups_ReturnsLastSetup()
        {
            var subject = new SetupMatcher(_setupManagerMock.Object,
                _targetMatcherMock.Object, _argumentMatcherMock.Object);

            var method = ReflectionUtility.GetMethod(() => Console.WriteLine());
            var arguments = new object[] { "Test" };

            var setups = TestDataFactory.CreateSetups(() => Console.WriteLine(), () => Console.WriteLine());
            _setupManagerMock.Setup(manager => manager.GetSetupsForMethod(method))
                .Returns(setups);

            _argumentMatcherMock
                .Setup(matcher => matcher.IsMatch(It.IsAny<IEnumerable<Expression>>(), arguments))
                .Returns(true);

            IInternalSetup result = subject.GetBestMatchingSetup(method, arguments);

            Assert.AreSame(setups[1], result);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetBestMatchingSetup_StaticMethodWithArguments_InvokesArgumentCheckerAndReturnsOnlyOnMatch(
            bool argumentsMatch)
        {
            var subject = new SetupMatcher(_setupManagerMock.Object,
                _targetMatcherMock.Object, _argumentMatcherMock.Object);

            var method = ReflectionUtility.GetMethod(() => Console.WriteLine("Test"));
            var arguments = new object[] { "Test" };

            var setups = TestDataFactory.CreateSetups(() => Console.WriteLine("Test"));
            _setupManagerMock.Setup(manager => manager.GetSetupsForMethod(method))
                .Returns(setups);

            _argumentMatcherMock
                .Setup(matcher => matcher.IsMatch(setups[0].MethodCall.Arguments, arguments))
                .Returns(argumentsMatch);

            IInternalSetup result = subject.GetBestMatchingSetup(method, arguments);

            if (argumentsMatch)
                Assert.AreSame(setups[0], result);
            else
                Assert.IsNull(result);
        }

        [SetUp]
        public void Setup()
        {
            SetupMocks(MockBehavior.Strict);
        }

        private void SetupMocks(MockBehavior behavior)
        {
            _argumentMatcherMock = new Mock<IArgumentMatcher>(behavior);
            _targetMatcherMock = new Mock<ITargetMatcher>(behavior);
            _setupManagerMock = new Mock<ISetupManager>(behavior);
        }
    }
}