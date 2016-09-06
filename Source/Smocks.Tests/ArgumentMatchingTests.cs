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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ArgumentMatchingTests
    {
        [TestCase]
        public void Setup_AnyReferenceTypeArgument_MatchesAnyInstance()
        {
            Smock.Run(context =>
            {
                const string expected = "Replaced!";

                context.Setup(() => string.Format("{0}", It.IsAny<string>())).Returns(expected);

                Assert.AreEqual(expected, string.Format("{0}", "One"));
                Assert.AreEqual(expected, string.Format("{0}", "Two"));
                Assert.AreEqual("Not replaced", string.Format("Not {0}", "replaced"));
            });
        }

        [TestCase]
        public void Setup_AnyValueTypeArgument_MatchesAnyInstance()
        {
            Smock.Run(context =>
            {
                context.Setup(() => Enumerable.Range(It.IsAny<int>(), 1)).Returns(new List<int>());

                Assert.AreEqual(0, Enumerable.Range(100, 1).Count());
                Assert.AreEqual(0, Enumerable.Range(200, 1).Count());
                Assert.AreEqual(2, Enumerable.Range(200, 2).Count());
            });
        }

        [TestCase(10)]
        public void Setup_ItIsAnyFromSmocks_MatchesAnyArgument(int expected)
        {
            Smock.Run(context =>
            {
                context.Setup(() => int.Parse(Matching.It.IsAny<string>())).Returns(expected);

                Assert.AreEqual(expected, int.Parse("42"));
            });
        }

        [TestCase(10)]
        public void Setup_ArgAnyFromNSubstitute_MatchesAnyArgument(int expected)
        {
            Smock.Run(context =>
            {
                context.Setup(() => int.Parse(Arg.Any<string>())).Returns(expected);

                Assert.AreEqual(expected, int.Parse("42"));
            });
        }

        [TestCase]
        public void Setup_ReferenceTypeArgument_MatchesOnlyEqualInstance()
        {
            Smock.Run(context =>
            {
                context.Setup(() => int.Parse("111")).Returns(10);

                Assert.AreEqual(10, int.Parse("111"));
                Assert.AreEqual(10, int.Parse(new string('1', 3)));
                Assert.AreEqual(100, int.Parse("100"));
            });
        }

        [TestCase]
        public void Setup_ValueTypeArgument_MatchesOnlyEqualInstance()
        {
            Smock.Run(context =>
            {
                context.Setup(() => Enumerable.Range(1, 1)).Returns(new List<int>());

                Assert.AreEqual(0, Enumerable.Range(1, 1).Count());
                Assert.AreEqual(2, Enumerable.Range(2, 2).Count());
            });
        }

        [TestCase]
        public void Setup_ParamsArray_MatchesWhenElementsInArrayMatch()
        {
            // params arrays are a strange beast: normally we only match reference 
            // types (such as arrays) when they are the *same* (ReferenceEquals).
            // For params arrays, we shouldn't actually treat them as array but match
            // their individual elements instead.
            Smock.Run(context =>
            {
                context.Setup(() => string.Format(string.Empty, "a", 1, 2, 3, 4)).Returns("It works!");

                var result = string.Format(string.Empty, "a", 1, 2, 3, 4);
                Assert.AreEqual("It works!", result);
            });
        }

        [TestCase]
        public void Setup_ItIsArgumentFromMoq_InvokesCallbackAndMatchesWhenItReturnsTrue()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.Format(It.Is<string>(format => format == "A"))).Returns("Foo");

                Assert.AreEqual("Foo", string.Format("A"));
            });
        }

        [TestCase]
        public void Setup_ArgIsArgumentFromNSubstitute_InvokesCallbackAndMatchesWhenItReturnsTrue()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.Format(Arg.Is<string>(format => format == "A"))).Returns("Foo");

                Assert.AreEqual("Foo", string.Format("A"));
            });
        }

        [TestCase]
        public void Setup_ItIsArgumentFromMoqWithMultipleConditions_InvokesCallbackAndMatchesWhenItReturnsTrue()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.Format(
                    It.Is<string>(format => format == "A" && format.Contains("A") && format.Length == 1))).Returns("Foo");

                Assert.AreEqual("Foo", string.Format("A"));
            });
        }

        [TestCase]
        public void Setup_MultipleItIsSetups_MatchesTheCorrectSetup()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.Format(It.Is<string>(format => format.Contains("A")))).Returns("Foo");
                context.Setup(() => string.Format(It.Is<string>(format => format.Contains("B")))).Returns("Bar");

                Assert.AreEqual("Foo", string.Format("AA"));
                Assert.AreEqual("Bar", string.Format("BB"));
                Assert.AreEqual("CC", string.Format("CC"));
            });
        }

        [TestCase]
        public void Setup_ItIsArgumentFromSmocks_InvokesCallbackAndMatchesWhenItReturnsTrue()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.Format(Matching.It.Is<string>(format => format == "A"))).Returns("Foo");

                Assert.AreEqual("Foo", string.Format("A"));
            });
        }

        [TestCase]
        public void Setup_ItIsArgumentFromMoq_InvokesCallbackAndInvokesOriginalWhenItReturnsFalse()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.Format(It.Is<string>(format => format == "A"))).Returns("Foo");

                Assert.AreEqual("B", string.Format("B"));
            });
        }
    }
}