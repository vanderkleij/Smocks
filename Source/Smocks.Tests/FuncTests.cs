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
using Moq;
using NUnit.Framework;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class FuncTests
    {
        [TestCase(true)]
        public void Run_BoolFunc_ReturnsCorrectValue(bool expected)
        {
            bool value = Smock.Run(context =>
            {
                context.Setup(() => bool.Parse(It.IsAny<string>())).Returns(expected);
                return bool.Parse("Invalid");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((byte)1)]
        public void Run_ByteFunc_ReturnsCorrectValue(byte expected)
        {
            byte value = Smock.Run(context =>
            {
                context.Setup(() => byte.Parse(It.IsAny<string>())).Returns(expected);
                return byte.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((char)1)]
        public void Run_CharFunc_ReturnsCorrectValue(char expected)
        {
            char value = Smock.Run(context =>
            {
                context.Setup(() => char.Parse(It.IsAny<string>())).Returns(expected);
                return char.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase]
        public void Run_DecimalFunc_ReturnsCorrectValue()
        {
            decimal expected = 1.23m;

            decimal value = Smock.Run(context =>
            {
                context.Setup(() => decimal.Parse(It.IsAny<string>())).Returns(expected);
                return decimal.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase(1.23)]
        public void Run_DoubleFunc_ReturnsCorrectValue(double expected)
        {
            double value = Smock.Run(context =>
            {
                context.Setup(() => double.Parse(It.IsAny<string>())).Returns(expected);
                return double.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase(1.23f)]
        public void Run_FloatFunc_ReturnsCorrectValue(float expected)
        {
            float value = Smock.Run(context =>
            {
                context.Setup(() => float.Parse(It.IsAny<string>())).Returns(expected);
                return float.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase]
        public void Run_GuidFunc_ReturnsCorrectValue()
        {
            Guid expected = Guid.Parse("{2988B6BD-B4D7-476D-B432-19D781FC9B20}");

            Guid value = Smock.Run(context =>
            {
                context.Setup(() => Guid.Parse(It.IsAny<string>())).Returns(expected);
                return Guid.Parse(string.Empty);
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase(1)]
        public void Run_IntFunc_ReturnsCorrectValue(int expected)
        {
            int value = Smock.Run(context =>
            {
                context.Setup(() => int.Parse(It.IsAny<string>())).Returns(expected);
                return int.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase]
        public void Run_ISerializableFunc_ReturnsCorrectValue()
        {
            Exception expected = new Exception("Test");

            Exception value = Smock.Run(context =>
            {
                context.Setup(() => new Exception(It.IsAny<string>())).Returns(expected);
                return new Exception(string.Empty);
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((long)1)]
        public void Run_LongFunc_ReturnsCorrectValue(long expected)
        {
            long value = Smock.Run(context =>
            {
                context.Setup(() => long.Parse(It.IsAny<string>())).Returns(expected);
                return long.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((sbyte)1)]
        public void Run_SByteFunc_ReturnsCorrectValue(sbyte expected)
        {
            sbyte value = Smock.Run(context =>
            {
                context.Setup(() => sbyte.Parse(It.IsAny<string>())).Returns(expected);
                return sbyte.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((short)1)]
        public void Run_ShortFunc_ReturnsCorrectValue(short expected)
        {
            short value = Smock.Run(context =>
            {
                context.Setup(() => short.Parse(It.IsAny<string>())).Returns(expected);
                return short.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((uint)1)]
        public void Run_UIntFunc_ReturnsCorrectValue(uint expected)
        {
            uint value = Smock.Run(context =>
            {
                context.Setup(() => uint.Parse(It.IsAny<string>())).Returns(expected);
                return uint.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((ulong)1)]
        public void Run_ULongFunc_ReturnsCorrectValue(ulong expected)
        {
            ulong value = Smock.Run(context =>
            {
                context.Setup(() => ulong.Parse(It.IsAny<string>())).Returns(expected);
                return ulong.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase]
        public void Run_UriFunc_ReturnsCorrectValue()
        {
            Uri expected = new Uri("http://www.google.com");

            Uri value = Smock.Run(context =>
            {
                context.Setup(() => new Uri(It.IsAny<string>())).Returns(expected);
                return new Uri(string.Empty);
            });

            Assert.AreEqual(expected, value);
        }

        [TestCase((ushort)1)]
        public void Run_UShortFunc_ReturnsCorrectValue(ushort expected)
        {
            ushort value = Smock.Run(context =>
            {
                context.Setup(() => ushort.Parse(It.IsAny<string>())).Returns(expected);
                return ushort.Parse("0");
            });

            Assert.AreEqual(expected, value);
        }
    }
}