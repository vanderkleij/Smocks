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
using NUnit.Framework;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests
{
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1508:ClosingCurlyBracketsMustNotBePrecededByBlankLine", Justification = "Generated.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1507:CodeMustNotContainMultipleBlankLinesInARow", Justification = "Generated.")]
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class CallbackExceptionTests
    {
        [TestCase]
        public void Callback_VoidLessThanExpectedOneArgument_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidOneArgument(1))
                        .Callback(() => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedOneArgument_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.OneArgument(1))
                        .Callback(() => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedOneArgument_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidOneArgument(1))
                        .Callback<int, int>((a, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedOneArgument_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.OneArgument(1))
                        .Callback<int, int>((a, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedTwoArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidTwoArguments(1, 2))
                        .Callback<int>((a) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedTwoArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TwoArguments(1, 2))
                        .Callback<int>((a) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedTwoArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidTwoArguments(1, 2))
                        .Callback<int, int, int>((a, b, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedTwoArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TwoArguments(1, 2))
                        .Callback<int, int, int>((a, b, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedThreeArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidThreeArguments(1, 2, 3))
                        .Callback<int, int>((a, b) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedThreeArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.ThreeArguments(1, 2, 3))
                        .Callback<int, int>((a, b) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedThreeArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidThreeArguments(1, 2, 3))
                        .Callback<int, int, int, int>((a, b, c, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedThreeArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.ThreeArguments(1, 2, 3))
                        .Callback<int, int, int, int>((a, b, c, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedFourArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFourArguments(1, 2, 3, 4))
                        .Callback<int, int, int>((a, b, c) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedFourArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FourArguments(1, 2, 3, 4))
                        .Callback<int, int, int>((a, b, c) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedFourArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFourArguments(1, 2, 3, 4))
                        .Callback<int, int, int, int, int>((a, b, c, d, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedFourArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FourArguments(1, 2, 3, 4))
                        .Callback<int, int, int, int, int>((a, b, c, d, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedFiveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFiveArguments(1, 2, 3, 4, 5))
                        .Callback<int, int, int, int>((a, b, c, d) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedFiveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FiveArguments(1, 2, 3, 4, 5))
                        .Callback<int, int, int, int>((a, b, c, d) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedFiveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFiveArguments(1, 2, 3, 4, 5))
                        .Callback<int, int, int, int, int, int>((a, b, c, d, e, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedFiveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FiveArguments(1, 2, 3, 4, 5))
                        .Callback<int, int, int, int, int, int>((a, b, c, d, e, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedSixArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidSixArguments(1, 2, 3, 4, 5, 6))
                        .Callback<int, int, int, int, int>((a, b, c, d, e) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedSixArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.SixArguments(1, 2, 3, 4, 5, 6))
                        .Callback<int, int, int, int, int>((a, b, c, d, e) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedSixArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidSixArguments(1, 2, 3, 4, 5, 6))
                        .Callback<int, int, int, int, int, int, int>((a, b, c, d, e, f, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedSixArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.SixArguments(1, 2, 3, 4, 5, 6))
                        .Callback<int, int, int, int, int, int, int>((a, b, c, d, e, f, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedSevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidSevenArguments(1, 2, 3, 4, 5, 6, 7))
                        .Callback<int, int, int, int, int, int>((a, b, c, d, e, f) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedSevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.SevenArguments(1, 2, 3, 4, 5, 6, 7))
                        .Callback<int, int, int, int, int, int>((a, b, c, d, e, f) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedSevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidSevenArguments(1, 2, 3, 4, 5, 6, 7))
                        .Callback<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedSevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.SevenArguments(1, 2, 3, 4, 5, 6, 7))
                        .Callback<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedEightArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidEightArguments(1, 2, 3, 4, 5, 6, 7, 8))
                        .Callback<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedEightArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.EightArguments(1, 2, 3, 4, 5, 6, 7, 8))
                        .Callback<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedEightArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidEightArguments(1, 2, 3, 4, 5, 6, 7, 8))
                        .Callback<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedEightArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.EightArguments(1, 2, 3, 4, 5, 6, 7, 8))
                        .Callback<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedNineArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidNineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9))
                        .Callback<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedNineArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.NineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9))
                        .Callback<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedNineArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidNineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9))
                        .Callback<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedNineArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.NineArguments(1, 2, 3, 4, 5, 6, 7, 8, 9))
                        .Callback<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedTenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidTenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
                        .Callback<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedTenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
                        .Callback<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedTenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidTenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedTenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedElevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
                        .Callback<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedElevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.ElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
                        .Callback<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedElevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedElevenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.ElevenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedTwelveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidTwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedTwelveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedTwelveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidTwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedTwelveArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TwelveArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedThirteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedThirteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.ThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedThirteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedThirteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.ThirteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedFourteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedFourteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedFourteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedFourteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FourteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedFifteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedFifteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidMoreThanExpectedFifteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidFifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_MoreThanExpectedFifteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.FifteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, z) => { });
                });
            });
        }

        [TestCase]
        public void Callback_VoidLessThanExpectedSixteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.VoidSixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { });
                });
            });
        }

        [TestCase]
        public void Callback_LessThanExpectedSixteenArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.SixteenArguments(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))
                        .Callback<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { });
                });
            });
        }


    }
}
