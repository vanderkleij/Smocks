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
using System.Text;
using NUnit.Framework;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ThrowsTests
    {
        [TestCase]
        public void Throws_ConstructorSetupToThrow_ThrowsSpecifiedException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => new StringBuilder())
                        .Throws<ArgumentException>();

                    Console.WriteLine(new StringBuilder());
                });
            });
        }

        [TestCase]
        public void Throws_ConstructorSetupToThrowSuppliedException_ThrowsSuppliedException()
        {
            var expected = new ArgumentException("Test");

            var actual = Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => new StringBuilder())
                        .Throws(expected);

                    Console.WriteLine(new StringBuilder());
                });
            });

            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestCase]
        public void Throws_MethodSetupToThrow_ThrowsSpecifiedException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => Console.WriteLine())
                        .Throws<InvalidOperationException>();

                    Console.WriteLine();
                });
            });
        }

        [TestCase]
        public void Throws_MethodSetupToThrowSuppliedException_ThrowsSuppliedException()
        {
            var expected = new InvalidOperationException("Test");

            var actual = Assert.Throws<InvalidOperationException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => Console.WriteLine())
                        .Throws(expected);

                    Console.WriteLine();
                });
            });

            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestCase]
        public void Throws_PropertySetupToThrow_ThrowsSpecifiedException()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => DateTime.Now)
                        .Throws<NotSupportedException>();

                    Console.WriteLine(DateTime.Now);
                });
            });
        }

        [TestCase]
        public void Throws_PropertySetupToThrowSuppliedException_ThrowsSuppliedException()
        {
            var expected = new NotSupportedException("Test");

            var actual = Assert.Throws<NotSupportedException>(() =>
            {
                Smock.Run(context =>
                {
                    context
                        .Setup(() => DateTime.Now)
                        .Throws(expected);

                    Console.WriteLine(DateTime.Now);
                });
            });

            Assert.AreEqual(expected.Message, actual.Message);
        }
    }
}