using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests
{
    [TestFixture]
    public partial class CallbackTests
    {
        [TestCase]
        public void Callback_OneMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.OneArgument(1))
                    .Callback<int>((a) => sum = a);

                TestFunctions.OneArgument(1);
                Assert.AreEqual(1, sum);
            });
        }

        [TestCase]
        public void Callback_VoidOneMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidOneArgument(1))
                    .Callback<int>((a) => sum = a);

                TestFunctions.VoidOneArgument(1);
                Assert.AreEqual(1, sum);
            });
        }

        [TestCase]
        public void Callback_TwoMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.TwoArguments(1, 2))
                    .Callback<int, int>((a, b) => sum = a + b);

                TestFunctions.TwoArguments(1, 2);
                Assert.AreEqual(3, sum);
            });
        }

        [TestCase]
        public void Callback_VoidTwoMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidTwoArguments(1, 2))
                    .Callback<int, int>((a, b) => sum = a + b);

                TestFunctions.VoidTwoArguments(1, 2);
                Assert.AreEqual(3, sum);
            });
        }

        [TestCase]
        public void Callback_ThreeMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.ThreeArguments(1, 2, 3))
                    .Callback<int, int, int>((a, b, c) => sum = a + b + c);

                TestFunctions.ThreeArguments(1, 2, 3);
                Assert.AreEqual(6, sum);
            });
        }

        [TestCase]
        public void Callback_VoidThreeMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidThreeArguments(1, 2, 3))
                    .Callback<int, int, int>((a, b, c) => sum = a + b + c);

                TestFunctions.VoidThreeArguments(1, 2, 3);
                Assert.AreEqual(6, sum);
            });
        }

        [TestCase]
        public void Callback_FourMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.FourArguments(1, 2, 3, 4))
                    .Callback<int, int, int, int>((a, b, c, d) => sum = a + b + c + d);

                TestFunctions.FourArguments(1, 2, 3, 4);
                Assert.AreEqual(10, sum);
            });
        }

        [TestCase]
        public void Callback_VoidFourMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidFourArguments(1, 2, 3, 4))
                    .Callback<int, int, int, int>((a, b, c, d) => sum = a + b + c + d);

                TestFunctions.VoidFourArguments(1, 2, 3, 4);
                Assert.AreEqual(10, sum);
            });
        }

        [TestCase]
        public void Callback_FiveMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.FiveArguments(1, 2, 3, 4, 5))
                    .Callback<int, int, int, int, int>((a, b, c, d, e) => sum = a + b + c + d + e);

                TestFunctions.FiveArguments(1, 2, 3, 4, 5);
                Assert.AreEqual(15, sum);
            });
        }

        [TestCase]
        public void Callback_VoidFiveMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidFiveArguments(1, 2, 3, 4, 5))
                    .Callback<int, int, int, int, int>((a, b, c, d, e) => sum = a + b + c + d + e);

                TestFunctions.VoidFiveArguments(1, 2, 3, 4, 5);
                Assert.AreEqual(15, sum);
            });
        }

        [TestCase]
        public void Callback_SixMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.SixArguments(1, 2, 3, 4, 5, 6))
                    .Callback<int, int, int, int, int, int>((a, b, c, d, e, f) => sum = a + b + c + d + e + f);

                TestFunctions.SixArguments(1, 2, 3, 4, 5, 6);
                Assert.AreEqual(21, sum);
            });
        }

        [TestCase]
        public void Callback_VoidSixMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidSixArguments(1, 2, 3, 4, 5, 6))
                    .Callback<int, int, int, int, int, int>((a, b, c, d, e, f) => sum = a + b + c + d + e + f);

                TestFunctions.VoidSixArguments(1, 2, 3, 4, 5, 6);
                Assert.AreEqual(21, sum);
            });
        }

        [TestCase]
        public void Callback_SevenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.SevenArguments(1, 2, 3, 4, 5, 6, 7))
                    .Callback<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => sum = a + b + c + d + e + f + g);

                TestFunctions.SevenArguments(1, 2, 3, 4, 5, 6, 7);
                Assert.AreEqual(28, sum);
            });
        }

        [TestCase]
        public void Callback_VoidSevenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidSevenArguments(1, 2, 3, 4, 5, 6, 7))
                    .Callback<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => sum = a + b + c + d + e + f + g);

                TestFunctions.VoidSevenArguments(1, 2, 3, 4, 5, 6, 7);
                Assert.AreEqual(28, sum);
            });
        }

        [TestCase]
        public void Callback_EightMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.EightArguments(1, 2, 3, 4, 5, 6, 7, 8))
                    .Callback<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => sum = a + b + c + d + e + f + g + h);

                TestFunctions.EightArguments(1, 2, 3, 4, 5, 6, 7, 8);
                Assert.AreEqual(36, sum);
            });
        }

        [TestCase]
        public void Callback_VoidEightMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidEightArguments(1, 2, 3, 4, 5, 6, 7, 8))
                    .Callback<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => sum = a + b + c + d + e + f + g + h);

                TestFunctions.VoidEightArguments(1, 2, 3, 4, 5, 6, 7, 8);
                Assert.AreEqual(36, sum);
            });
        }

        [TestCase]
        public void Callback_NineMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.NineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9))
                    .Callback<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => sum = a + b + c + d + e + f + g + h + i);

                TestFunctions.NineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9);
                Assert.AreEqual(45, sum);
            });
        }

        [TestCase]
        public void Callback_VoidNineMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidNineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9))
                    .Callback<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => sum = a + b + c + d + e + f + g + h + i);

                TestFunctions.VoidNineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9);
                Assert.AreEqual(45, sum);
            });
        }

        [TestCase]
        public void Callback_TenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.TenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
                    .Callback<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => sum = a + b + c + d + e + f + g + h + i + j);

                TestFunctions.TenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
                Assert.AreEqual(55, sum);
            });
        }

        [TestCase]
        public void Callback_VoidTenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidTenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
                    .Callback<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => sum = a + b + c + d + e + f + g + h + i + j);

                TestFunctions.VoidTenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
                Assert.AreEqual(55, sum);
            });
        }

        [TestCase]
        public void Callback_ElevenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.ElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => sum = a + b + c + d + e + f + g + h + i + j + k);

                TestFunctions.ElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
                Assert.AreEqual(66, sum);
            });
        }

        [TestCase]
        public void Callback_VoidElevenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => sum = a + b + c + d + e + f + g + h + i + j + k);

                TestFunctions.VoidElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
                Assert.AreEqual(66, sum);
            });
        }

        [TestCase]
        public void Callback_TwelveMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.TwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => sum = a + b + c + d + e + f + g + h + i + j + k + l);

                TestFunctions.TwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
                Assert.AreEqual(78, sum);
            });
        }

        [TestCase]
        public void Callback_VoidTwelveMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidTwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => sum = a + b + c + d + e + f + g + h + i + j + k + l);

                TestFunctions.VoidTwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
                Assert.AreEqual(78, sum);
            });
        }

        [TestCase]
        public void Callback_ThirteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.ThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m);

                TestFunctions.ThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
                Assert.AreEqual(91, sum);
            });
        }

        [TestCase]
        public void Callback_VoidThirteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m);

                TestFunctions.VoidThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
                Assert.AreEqual(91, sum);
            });
        }

        [TestCase]
        public void Callback_FourteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.FourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m + n);

                TestFunctions.FourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
                Assert.AreEqual(105, sum);
            });
        }

        [TestCase]
        public void Callback_VoidFourteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidFourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m + n);

                TestFunctions.VoidFourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
                Assert.AreEqual(105, sum);
            });
        }

        [TestCase]
        public void Callback_FifteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.FifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m + n + o);

                TestFunctions.FifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
                Assert.AreEqual(120, sum);
            });
        }

        [TestCase]
        public void Callback_VoidFifteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidFifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m + n + o);

                TestFunctions.VoidFifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
                Assert.AreEqual(120, sum);
            });
        }

        [TestCase]
        public void Callback_SixteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.SixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p);

                TestFunctions.SixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
                Assert.AreEqual(136, sum);
            });
        }

        [TestCase]
        public void Callback_VoidSixteenMatchingArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int sum = 0;
                context
                    .Setup(() => TestFunctions.VoidSixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))
                    .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => sum = a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p);

                TestFunctions.VoidSixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
                Assert.AreEqual(136, sum);
            });
        }

	}
}
