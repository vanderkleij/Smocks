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
using Moq;
using NUnit.Framework;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests
{
    /// <summary>
    /// Tests for the .Callback() methods.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public partial class CallbackTests
    {
        [TestCase]
        public void Callback_Constructor_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                string actual = null;

                context
                    .Setup(() => new Exception(It.IsAny<string>()))
                    .Callback<string>(arg => actual = arg);

                new Exception("Foo");

                Assert.AreEqual("Foo", actual);
            });
        }

        [TestCase]
        public void Callback_InstanceMethodWithOneArgument_InvokesCallbackWithOneArgument()
        {
            Smock.Run(context =>
            {
                string argument = null;

                context
                    .Setup(() => It.IsAny<string>().Contains(It.IsAny<string>()))
                    .Callback<string>(arg => argument = arg);

                "Foo".Contains("Bar");

                Assert.AreEqual("Bar", argument);
            });
        }

        [TestCase]
        public void Callback_NoArgs_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                bool invoked = false;

                context.Setup(() => Console.ReadLine())
                    .Callback(() => invoked = true)
                    .Returns(string.Empty);

                Console.ReadLine();

                Assert.IsTrue(invoked);
            });
        }

        [TestCase]
        public void Callback_SingleConvertibleArg_InvokesCallbackWithArguments()
        {
            Smock.Run(context =>
            {
                int result = 0;
                context.Setup(() => Math.Sqrt(It.IsAny<double>())).Callback<int>(d => result = d * d);

                Math.Sqrt(2.0);

                Assert.AreEqual(4, result);
            });
        }

        [TestCase]
        public void Callback_SingleNonConvertibleArg_ThrowsException()
        {
            Smock.Run(context =>
            {
                context.Setup(() => string.IsNullOrEmpty("Test")).Callback<IDisposable>(disposable => { });

                Assert.Throws<InvalidCastException>(() => string.IsNullOrEmpty("Test"));
            });
        }

        [TestCase]
        public void Callback_TooFewArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.TwoArguments(1, 2))
                        .Callback<int>(a => { });
                });
            });
        }

        [TestCase]
        public void Callback_TooFewVoidMethodArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => Console.WriteLine("{0}", 1))
                        .Callback<string>(a => { });
                });
            });
        }

        [TestCase]
        public void Callback_TooManyArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => TestFunctions.OneArgument(1))
                        .Callback<int, int>((a, b) => { });
                });
            });
        }

        [TestCase]
        public void Callback_TooManyVoidMethodArguments_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => Console.WriteLine("{0}"))
                        .Callback<string, int>((a, b) => { });
                });
            });
        }

        [TestCase]
        public void Callback_TwoConvertibleArgs_InvokesCallbackWithArguments()
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
        public void Callback_VoidMethod_InvokesCallback()
        {
            bool invoked = false;

            Smock.Run(context =>
            {
                context
                    .Setup(() => Console.WriteLine(It.IsAny<string>()))
                    .Callback<string>(s => invoked = true);

                Console.WriteLine("Hello!");
            });

            Assert.IsTrue(invoked);
        }
    }
}