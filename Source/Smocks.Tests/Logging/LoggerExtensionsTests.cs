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
using Smocks.Logging;

namespace Smocks.Tests.Logging
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class LoggerExtensionsTests
    {
        private Mock<Logger> _loggerMock;

        [TestCase]
        public void Debug_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Debug(string.Empty));
#pragma warning restore 1720
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Debug_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Debug, totalMessage)).Verifiable();
            _loggerMock.Object.Debug(format, argument);
            _loggerMock.Verify();
        }

        [TestCase]
        public void Error_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Error(string.Empty));
#pragma warning restore 1720
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Error_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Error, totalMessage)).Verifiable();
            _loggerMock.Object.Error(format, argument);
            _loggerMock.Verify();
        }

        [TestCase]
        public void Fatal_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Fatal(string.Empty));
#pragma warning restore 1720
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Fatal_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Fatal, totalMessage)).Verifiable();
            _loggerMock.Object.Fatal(format, argument);
            _loggerMock.Verify();
        }

        [TestCase]
        public void Info_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Info(string.Empty));
#pragma warning restore 1720
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Info_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Info, totalMessage)).Verifiable();
            _loggerMock.Object.Info(format, argument);
            _loggerMock.Verify();
        }

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<Logger>(MockBehavior.Strict);
        }

        [TestCase]
        public void Warn_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Warn(string.Empty));
#pragma warning restore 1720
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Warn_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Warn, totalMessage)).Verifiable();
            _loggerMock.Object.Warn(format, argument);
            _loggerMock.Verify();
        }
    }
}