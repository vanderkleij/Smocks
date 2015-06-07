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
    public class ArgumentMatcherTests
    {
        private static readonly Expression AnyStringExpression =
            ReflectionUtility.GetExpression(() => It.IsAny<string>());

        private Mock<IExpressionHelper> _expressionHelperMock;

        [TestCase]
        public void Constructor_ExpressionHelperNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ArgumentMatcher(null));
            Assert.AreEqual("expressionHelper", exception.ParamName);
        }

        [TestCase]
        public void IsMatch_AllSetupArgumentsAny_ReturnsTrue()
        {
            var subject = new ArgumentMatcher(_expressionHelperMock.Object);

            var setupArguments = new[] { AnyStringExpression, AnyStringExpression };
            var actualArguments = new[] { "Hello", "World" };

            var result = subject.IsMatch(setupArguments, actualArguments);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void IsMatch_AllSetupArgumentsEqual_ReturnsTrue()
        {
            var subject = new ArgumentMatcher(_expressionHelperMock.Object);

            var setupArguments = new[] { Expression.Constant("Hello"), Expression.Constant("World") };
            var actualArguments = new[] { "Hello", "World" };

            var result = subject.IsMatch(setupArguments, actualArguments);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void IsMatch_SomeSetupArgumentUnequal_ReturnsFalse()
        {
            var subject = new ArgumentMatcher(_expressionHelperMock.Object);

            var setupArguments = new[] { Expression.Constant("Hello"), Expression.Constant("Qwerty") };
            var actualArguments = new[] { "Hello", "World" };

            var result = subject.IsMatch(setupArguments, actualArguments);

            Assert.IsFalse(result);
        }

        [SetUp]
        public void Setup()
        {
            _expressionHelperMock = new Mock<IExpressionHelper>();
            _expressionHelperMock
                .Setup(helper => helper.IsUnconditionalAny(It.IsAny<Expression>()))
                .Returns<Expression>(expression => expression == AnyStringExpression);

            _expressionHelperMock
                .Setup(helper => helper.GetValue(It.IsAny<ConstantExpression>()))
                .Returns<ConstantExpression>(expression => expression.Value);
        }
    }
}