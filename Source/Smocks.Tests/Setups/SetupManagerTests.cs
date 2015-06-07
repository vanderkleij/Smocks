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
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Smocks.Setups;
using Smocks.Tests.TestUtility;
using Smocks.Utility;

namespace Smocks.Tests.Setups
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SetupManagerTests
    {
        private Mock<IExpressionHelper> _expressionHelperMock;

        [TestCase]
        public void Constructor_ExpressionHelperNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new SetupManager(null));
            Assert.AreEqual("expressionHelper", exception.ParamName);
        }

        [TestCase]
        public void CreateFromAction_AddsToCollection()
        {
            var methodCallInfo = TestDataFactory.CreateMethodCallInfo(() => Console.WriteLine());
            Expression<Action> expression = ReflectionUtility.GetExpression(() => Console.WriteLine());

            _expressionHelperMock
                .Setup(helper => helper.GetMethod(expression))
                .Returns(methodCallInfo);

            var subject = new SetupManager(_expressionHelperMock.Object);
            var setup = subject.Create(expression);

            Assert.IsTrue(subject.SequenceEqual(new[] { setup }));
        }

        [TestCase]
        public void CreateFromAction_ExpressionHelperReturnsNullMethod_ThrowsException()
        {
            Expression<Action> expression = ReflectionUtility.GetExpression(() => Console.WriteLine());
            _expressionHelperMock
                .Setup(helper => helper.GetMethod(expression))
                .Returns(default(MethodCallInfo));

            var subject = new SetupManager(_expressionHelperMock.Object);

            var exception = Assert.Throws<ArgumentException>(() => subject.Create(expression));
            Assert.AreEqual("expression", exception.ParamName);
        }

        [TestCase]
        public void CreateFromAction_ReturnsSetupFromExpressionHelper()
        {
            var methodCallInfo = TestDataFactory.CreateMethodCallInfo(() => Console.WriteLine());

            Expression<Action> expression = ReflectionUtility.GetExpression(() => Console.WriteLine());
            _expressionHelperMock
                .Setup(helper => helper.GetMethod(expression))
                .Returns(methodCallInfo);

            var subject = new SetupManager(_expressionHelperMock.Object);
            var result = subject.Create(expression) as IInternalSetup;

            Assert.AreSame(result.MethodCall, methodCallInfo);
        }

        [TestCase]
        public void CreateFromFunc_AddsToCollection()
        {
            var methodCallInfo = TestDataFactory.CreateMethodCallInfo(() => Console.ReadLine());

            var expression = ReflectionUtility.GetExpression(() => Console.ReadLine());
            _expressionHelperMock
                .Setup(helper => helper.GetMethod(expression))
                .Returns(methodCallInfo);

            var subject = new SetupManager(_expressionHelperMock.Object);
            var setup = subject.Create(expression) as IInternalSetup;

            Assert.IsTrue(subject.SequenceEqual(new[] { setup }));
        }

        [TestCase]
        public void CreateFromFunc_ExpressionHelperReturnsNullMethod_ThrowsException()
        {
            var expression = ReflectionUtility.GetExpression(() => Console.ReadLine());
            _expressionHelperMock
                .Setup(helper => helper.GetMethod(expression))
                .Returns(default(MethodCallInfo));

            var subject = new SetupManager(_expressionHelperMock.Object);

            var exception = Assert.Throws<ArgumentException>(() => subject.Create(expression));
            Assert.AreEqual("expression", exception.ParamName);
        }

        [TestCase]
        public void CreateFromFunc_ReturnsSetupFromExpressionHelper()
        {
            var methodCallInfo = TestDataFactory.CreateMethodCallInfo(() => Console.ReadLine());

            var expression = ReflectionUtility.GetExpression(() => Console.ReadLine());
            _expressionHelperMock
                .Setup(helper => helper.GetMethod(expression))
                .Returns(methodCallInfo);

            var subject = new SetupManager(_expressionHelperMock.Object);
            var result = subject.Create(expression) as IInternalSetup;

            Assert.AreSame(result.MethodCall, methodCallInfo);
        }

        [TestCase]
        public void CreateFromProperty_ReturnsSetupFromExpressionHelper()
        {
            // Doesn't really matter what method call we create here, it's only
            // used for comparison.
            var methodCallInfo = TestDataFactory.CreateMethodCallInfo(() => Console.WriteLine());

            var expression = ReflectionUtility.GetExpression(() => DateTime.Now);
            _expressionHelperMock
                .Setup(helper => helper.GetPropertyGetCall(expression))
                .Returns(methodCallInfo);

            var subject = new SetupManager(_expressionHelperMock.Object);
            var result = subject.Create(expression) as IInternalSetup;

            Assert.AreSame(result.MethodCall, methodCallInfo);
        }

        [TestCase]
        public void GetSetupsForMethod_ReturnsOnlyMatchingSetups()
        {
            var methodCallInfo = TestDataFactory.CreateMethodCallInfo(() => Console.WriteLine());
            Expression<Action> expression = ReflectionUtility.GetExpression(() => Console.WriteLine());

            _expressionHelperMock
                .Setup(helper => helper.GetMethod(expression))
                .Returns(methodCallInfo);

            var subject = new SetupManager(_expressionHelperMock.Object);
            subject.Create(expression);

            var result1 = subject.GetSetupsForMethod(ReflectionUtility.GetMethod(() => Console.WriteLine()));
            var result2 = subject.GetSetupsForMethod(ReflectionUtility.GetMethod(() => Console.ReadLine()));

            Assert.AreEqual(1, result1.Count);
            Assert.AreEqual(0, result2.Count);
        }

        [SetUp]
        public void Setup()
        {
            _expressionHelperMock = new Mock<IExpressionHelper>();
        }
    }
}