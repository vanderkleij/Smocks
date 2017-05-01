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
using System.Text;
using NUnit.Framework;
using Smocks.Tests.TestUtility;
using Smocks.Utility;

namespace Smocks.Tests.Utility
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class MethodCallInfoTests
    {
        [TestCase]
        public void Equals_OtherDifferentMethod_ReturnsFalse()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());
            MethodCallInfo other = CreateSubject(() => Console.ReadLine());

            bool result = subject.Equals(other);

            Assert.IsFalse(result);
        }

        [TestCase]
        public void Equals_OtherNull_ReturnsFalse()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());

            bool result = subject.Equals(default(MethodCallInfo));

            Assert.IsFalse(result);
        }

        [TestCase]
        public void Equals_OtherSameInstance_ReturnsTrue()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());

            bool result = subject.Equals(subject);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void Equals_OtherSameMethod_ReturnsTrue()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());
            MethodCallInfo other = CreateSubject(() => Console.WriteLine());

            bool result = subject.Equals(other);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void EqualsObject_OtherDifferentMethod_ReturnsFalse()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());
            MethodCallInfo other = CreateSubject(() => Console.ReadLine());

            bool result = subject.Equals((object)other);

            Assert.IsFalse(result);
        }

        [TestCase]
        public void EqualsObject_OtherDifferentType_ReturnsFalse()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());
            object other = new StringBuilder();

            bool result = subject.Equals(other);

            Assert.IsFalse(result);
        }

        [TestCase]
        public void EqualsObject_OtherNull_ReturnsFalse()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());

            bool result = subject.Equals(default(object));

            Assert.IsFalse(result);
        }

        [TestCase]
        public void EqualsObject_OtherSameInstance_ReturnsTrue()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());

            bool result = subject.Equals((object)subject);

            Assert.IsTrue(result);
        }

        [TestCase]
        public void EqualsObject_OtherSameMethod_ReturnsTrue()
        {
            MethodCallInfo subject = CreateSubject(() => Console.WriteLine());
            MethodCallInfo other = CreateSubject(() => Console.WriteLine());

            bool result = subject.Equals((object)other);

            Assert.IsTrue(result);
        }

        [SetUp]
        public void Setup()
        {
        }

        private MethodCallInfo CreateSubject(Expression<Action> expression)
        {
            return new MethodCallInfo(ReflectionUtility.GetMethod(expression));
        }
    }
}