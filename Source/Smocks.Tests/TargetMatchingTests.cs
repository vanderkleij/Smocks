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

using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TargetMatchingTests
    {
        [TestCase]
        public void Setup_AnyReferenceTypeTarget_MatchesAnyInstance()
        {
            Smock.Run(context =>
            {
                const int expected = 1337;

                context.Setup(() => It.IsAny<string>().Length).Returns(expected);

                Assert.AreEqual(expected, "Hello".Length);
                Assert.AreEqual(expected, "World".Length);
            });
        }

        [TestCase]
        public void Setup_AnyValueTypeTarget_MatchesAnyInstance()
        {
            Smock.Run(context =>
            {
                const int expected = 1337;

                context.Setup(() => It.IsAny<int>().GetHashCode()).Returns(expected);

                Assert.AreEqual(expected, 1.GetHashCode());
                Assert.AreEqual(expected, 2.GetHashCode());
            });
        }

        [TestCase]
        public void Setup_ReferenceTypeTarget_MatchesOnlySameInstance()
        {
            Smock.Run(context =>
            {
                const string target = "ccccc";

                context.Setup(() => target.Length).Returns(50);

                Assert.AreEqual(50, target.Length);
                Assert.AreEqual(5, new string('c', 5).Length);
                Assert.AreEqual(4, "Four".Length);
            });
        }

        [TestCase(1, 2)]
        public void Setup_ValueTypeTarget_MatchesOnlyEqualInstance(int valueType, int otherValueType)
        {
            Smock.Run(context =>
            {
                const int expected = 1337;

                context.Setup(() => valueType.GetHashCode()).Returns(expected);

                Assert.AreEqual(expected, valueType.GetHashCode());
                Assert.AreNotEqual(expected, otherValueType.GetHashCode());
            });
        }
    }
}