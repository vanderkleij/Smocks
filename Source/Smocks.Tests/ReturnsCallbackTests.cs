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
using System.Globalization;
using System.Linq;
using System.Net;
using Moq;
using NUnit.Framework;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ReturnsCallbackTests
    {
        [TestCase]
        public void ReturnsCallback_ConstructorWithArguments_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => new Exception(It.IsAny<string>()))
                    .Returns<string>(arg => new ArgumentException(arg + "Bar"));

                var actual = new Exception("Foo");

                Assert.IsTrue(actual is ArgumentException);
                Assert.AreEqual("FooBar", actual.Message);
            });
        }

        [TestCase]
        public void ReturnsCallback_InstanceMethodWithOneArgument_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                string argument = null;

                context
                    .Setup(() => It.IsAny<string>().Contains(It.IsAny<string>()))
                    .Returns<string>(arg => 
                    {
                        argument = arg;
                        return true;
                    });

                bool result = "Foo".Contains("Bar");

                Assert.AreEqual("Bar", argument);
                Assert.IsTrue(result);
            });
        }

        [TestCase]
        public void ReturnsCallback_EightMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.EightArguments(1, 2, 3, 4, 5, 6, 7, 8))
                    .Returns<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);

                var result = TestFunctions.EightArguments(1, 2, 3, 4, 5, 6, 7, 8);
                Assert.AreEqual(36, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_ElevenMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.ElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
                    .Returns<int, int, int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k);

                var result = TestFunctions.ElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
                Assert.AreEqual(66, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_FifteenMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.FifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
                    .Returns<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o);

                var result = TestFunctions.FifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
                Assert.AreEqual(120, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_FiveMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.FiveArguments(1, 2, 3, 4, 5))
                    .Returns<int, int, int, int, int>((a, b, c, d, e) => a + b + c + d + e);

                var result = TestFunctions.FiveArguments(1, 2, 3, 4, 5);
                Assert.AreEqual(15, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_FourMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.FourArguments(1, 2, 3, 4))
                    .Returns<int, int, int, int>((a, b, c, d) => a + b + c + d);

                var result = TestFunctions.FourArguments(1, 2, 3, 4);
                Assert.AreEqual(10, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_FourteenMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.FourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
                    .Returns<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n);

                var result = TestFunctions.FourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
                Assert.AreEqual(105, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_NineMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.NineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9))
                    .Returns<int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i);

                var result = TestFunctions.NineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9);
                Assert.AreEqual(45, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_NoArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                const string expected = "ExpectedValue";

                context.Setup(() => Dns.GetHostName()).Returns(() => expected);

                Assert.AreEqual(expected, Dns.GetHostName());
            });
        }

        [TestCase]
        public void ReturnsCallback_SevenMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.SevenArguments(1, 2, 3, 4, 5, 6, 7))
                    .Returns<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => a + b + c + d + e + f + g);

                var result = TestFunctions.SevenArguments(1, 2, 3, 4, 5, 6, 7);
                Assert.AreEqual(28, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_SingleConvertibleArg_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.Copy(It.IsAny<string>())).Returns<double>(d => (d * d).ToString(CultureInfo.InvariantCulture));

                Assert.AreEqual("4", string.Copy("2"));
            });
        }

        [TestCase]
        public void ReturnsCallback_SingleMatchingArg_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context.Setup(() => int.Parse(It.IsAny<string>())).Returns<string>(s => s.Length);

                Assert.AreEqual(4, int.Parse("Five"));
                Assert.AreEqual(3, int.Parse("Six"));
            });
        }

        [TestCase]
        public void ReturnsCallback_TooManyArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.OneArgument(1))
                        .Returns<int, int>((a, b) => 42);
                });
            });
        }

        [TestCase]
        public void ReturnsCallback_TooFewArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TwoArguments(1, 2))
                        .Returns<int>(a => 42);
                });
            });
        }

        [TestCase]
        public void ReturnsCallback_SingleNonConvertibleArg_ThrowsException()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.IsNullOrEmpty(It.IsAny<string>())).Returns<IDisposable>(disposable => true);

                Assert.Throws<InvalidCastException>(() => string.IsNullOrEmpty("test"));
            });
        }

        [TestCase]
        public void ReturnsCallback_SixMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.SixArguments(1, 2, 3, 4, 5, 6))
                    .Returns<int, int, int, int, int, int>((a, b, c, d, e, f) => a + b + c + d + e + f);

                var result = TestFunctions.SixArguments(1, 2, 3, 4, 5, 6);
                Assert.AreEqual(21, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_SixteenMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.SixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))
                    .Returns<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p);

                var result = TestFunctions.SixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
                Assert.AreEqual(136, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_TenMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.TenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
                    .Returns<int, int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j);

                var result = TestFunctions.TenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
                Assert.AreEqual(55, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_ThirteenMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.ThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
                    .Returns<int, int, int, int, int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m);

                var result = TestFunctions.ThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
                Assert.AreEqual(91, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_ThreeMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.ThreeArguments(1, 2, 3))
                    .Returns<int, int, int>((a, b, c) => a + b + c);

                Assert.AreEqual(6, TestFunctions.ThreeArguments(1, 2, 3));
            });
        }

        [TestCase]
        public void ReturnsCallback_TwelveMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context
                    .Setup(() => TestFunctions.TwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
                    .Returns<int, int, int, int, int, int, int, int, int, int, int, int>(
                        (a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l);

                var result = TestFunctions.TwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
                Assert.AreEqual(78, result);
            });
        }

        [TestCase]
        public void ReturnsCallback_TwoConvertibleArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context.Setup(() => Enumerable.Range(2, 3)).Returns<double, long>((a, b) => new[] { (int)(a * b) });

                Assert.IsTrue(Enumerable.Range(2, 3).SequenceEqual(new[] { 6 }));
            });
        }

        [TestCase]
        public void ReturnsCallback_TwoMatchingArgs_ReturnsResultFromCallback()
        {
            Smock.Run(context =>
            {
                context.Setup(() => Enumerable.Range(2, 3)).Returns<int, int>((a, b) => new[] { a * b });

                Assert.IsTrue(Enumerable.Range(2, 3).SequenceEqual(new[] { 6 }));
            });
        }
    }
}