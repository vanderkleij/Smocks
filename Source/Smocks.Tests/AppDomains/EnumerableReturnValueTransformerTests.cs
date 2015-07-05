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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Smocks.AppDomains;

namespace Smocks.Tests.AppDomains
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class EnumerableReturnValueTransformerTests
    {
        [TestCase]
        public void CanTransform_IEnumerable_ReturnsTrue()
        {
            var subject = new EnumerableReturnValueTransformer();
            var result = subject.CanTransform(typeof(IEnumerable), null);

            Assert.IsTrue(result);
        }

        [TestCase(typeof(List<int>))]
        [TestCase(typeof(IEnumerable<int>))]
        public void CanTransform_NotIEnumerable_ReturnsFalse(Type type)
        {
            var subject = new EnumerableReturnValueTransformer();
            var result = subject.CanTransform(type, null);

            Assert.IsFalse(result);
        }

        [TestCase]
        public void Transform_IEnumerable_ReturnsGenericList()
        {
            var list = new ArrayList { 1, 2, 3 };

            var subject = new EnumerableReturnValueTransformer();
            var result = subject.Transform<IEnumerable>(list);

            Assert.NotNull(result);
            Assert.IsTrue(result is List<object>);
            Assert.AreEqual(list.Count, result.Cast<object>().Count());
        }

        [TestCase]
        public void Transform_Null_ReturnsNull()
        {
            var subject = new EnumerableReturnValueTransformer();
            var result = subject.Transform<IEnumerable>(null);

            Assert.IsNull(result);
        }
    }
}