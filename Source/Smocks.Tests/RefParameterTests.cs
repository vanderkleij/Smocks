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
using Smocks.Tests.TestUtility;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class RefParameterTests
    {
        [TestCase]
        public void Setup_MatchWithInt32RefParameter_ReturnsConfiguredValue()
        {
            Smock.Run(context =>
            {
                int refSetupValue = 10;
                context.Setup(() => TestFunctions.IntRefParameter(It.IsAny<string>(), ref refSetupValue)).Returns(true);

                int refValue = refSetupValue;
                bool result = TestFunctions.IntRefParameter("SomeString", ref refValue);

                Assert.AreEqual(10, refSetupValue);
                Assert.AreEqual(true, result);
            });
        }

        [TestCase]
        public void Setup_MatchWithInt64RefParameter_ReturnsConfiguredValue()
        {
            Smock.Run(context =>
            {
                long refSetupValue = 42;
                context.Setup(() => TestFunctions.LongRefParameter(It.IsAny<string>(), ref refSetupValue)).Returns(true);

                long refValue = refSetupValue;
                bool result = TestFunctions.LongRefParameter("SomeString", ref refValue);

                Assert.AreEqual(42, refSetupValue);
                Assert.AreEqual(true, result);
            });
        }

        [TestCase]
        public void Setup_MatchWithReferenceTypeOutParameter_SetsOutParameterToSetupValue()
        {
            Smock.Run(context =>
            {
                string refSetupValue = "Foo";
                context.Setup(() => TestFunctions.StringRefParameter(It.IsAny<string>(), ref refSetupValue)).Returns(true);

                string refValue = refSetupValue;
                bool result = TestFunctions.StringRefParameter("SomeString", ref refValue);

                Assert.AreEqual("Foo", refSetupValue);
                Assert.AreEqual(true, result);
            });
        }

        [TestCase]
        public void Setup_NoMatchWithValueTypeRefParameter_InvokesOriginalMethod()
        {
            Smock.Run(context =>
            {
                int refSetupValue = 10;
                context.Setup(() => TestFunctions.IntRefParameter(It.IsAny<string>(), ref refSetupValue)).Returns(true);

                int refValue = 100;
                bool result = TestFunctions.IntRefParameter("SomeString", ref refValue);

                // IntRefParameter adds 10 to the supplied value and returns false.
                Assert.AreEqual(110, refValue);
                Assert.AreEqual(false, result);
            });
        }
    }
}