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

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Smocks.Matching;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class OutParameterTests
    {
        [TestCase]
        public void Setup_MatchWithInt32OutParameter_SetsOutParameterToSetupValue()
        {
            Smock.Run(context =>
            {
                int expectedOutResult = 10;
                context.Setup(() => int.TryParse(It.IsAny<string>(), out expectedOutResult)).Returns(true);

                int outResult;
                bool result = int.TryParse("InvalidNumber", out outResult);

                Assert.AreEqual(expectedOutResult, outResult);
                Assert.AreEqual(true, result);
            });
        }

        [TestCase]
        public void Setup_MatchWithInt64OutParameter_SetsOutParameterToSetupValue()
        {
            Smock.Run(context =>
            {
                long expectedOutResult = 10;
                context.Setup(() => long.TryParse(It.IsAny<string>(), out expectedOutResult)).Returns(true);

                long outResult;
                bool result = long.TryParse("InvalidNumber", out outResult);

                Assert.AreEqual(expectedOutResult, outResult);
                Assert.AreEqual(true, result);
            });
        }

        [TestCase]
        public void Setup_NoMatchWithValueTypeOutParameter_InvokesOriginalMethod()
        {
            Smock.Run(context =>
            {
                int expectedOutResult = 10;
                context.Setup(() => int.TryParse("SomeOtherValue", out expectedOutResult)).Returns(true);

                int outResult;
                bool result = int.TryParse("200", out outResult);

                Assert.AreEqual(200, outResult);
                Assert.AreEqual(true, result);
            });
        }
    }
}