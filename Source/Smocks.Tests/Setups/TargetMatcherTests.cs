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
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks.Tests.Setups
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TargetMatcherTests
    {
        private Mock<IExpressionHelper> _expressionHelperMock;
        private Mock<IItIsMatcher> _itIsMatcherMock;

        [TestCase]
        public void Constructor_ExpressionHelperNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TargetMatcher(null, _itIsMatcherMock.Object));
            Assert.AreEqual("expressionHelper", exception.ParamName);
        }

        [TestCase]
        public void Constructor_ItIsMatcherNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TargetMatcher(_expressionHelperMock.Object, null));
            Assert.AreEqual("itIsMatcher", exception.ParamName);
        }

        [TestCase]
        public void IsMatch_IsUnconditionalAny_ReturnsTrue()
        {
            var expression = Expression.Constant(1);

            _expressionHelperMock
                .Setup(helper => helper.IsMethodInvocation(expression, "It", "IsAny", 0))
                .Returns(true);

            var subject = new TargetMatcher(_expressionHelperMock.Object, _itIsMatcherMock.Object);
            var result = subject.IsMatch(typeof(object), expression, 42);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void IsMatch_ReferenceTypeTargetEqualToActualButNotSame_ReturnsFalse()
        {
            var expression = Expression.Constant(1);

            SetupExpressionValue(expression, "aaa");

            var subject = new TargetMatcher(_expressionHelperMock.Object, _itIsMatcherMock.Object);
            var result = subject.IsMatch(typeof(string), expression, new string('a', 3));

            Assert.IsFalse(result);
        }

        [TestCase]
        public void IsMatch_ReferenceTypeTargetSameAsActual_ReturnsTrue()
        {
            const string value = "aaa";

            var expression = Expression.Constant(1);

            SetupExpressionValue(expression, value);

            var subject = new TargetMatcher(_expressionHelperMock.Object, _itIsMatcherMock.Object);
            var result = subject.IsMatch(typeof(string), expression, value);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void IsMatch_ValueTypeTargetEqualToActual_ReturnsTrue()
        {
            var expression = Expression.Constant(1);

            SetupExpressionValue(expression, 42);

            var subject = new TargetMatcher(_expressionHelperMock.Object, _itIsMatcherMock.Object);
            var result = subject.IsMatch(typeof(int), expression, 42);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void IsMatch_ValueTypeTargetUnequalToActual_ReturnsFalse()
        {
            var expression = Expression.Constant(1);

            SetupExpressionValue(expression, 1000);

            var subject = new TargetMatcher(_expressionHelperMock.Object, _itIsMatcherMock.Object);
            var result = subject.IsMatch(typeof(int), expression, 42);

            Assert.IsFalse(result);
        }

        [SetUp]
        public void Setup()
        {
            _expressionHelperMock = new Mock<IExpressionHelper>();
            _itIsMatcherMock = new Mock<IItIsMatcher>();
        }

        private void SetupExpressionValue<T>(ConstantExpression expression, T value)
        {
            _expressionHelperMock
                .Setup(helper => helper.GetValue(expression))
                .Returns(value);
        }
    }
}