using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<Logger>(MockBehavior.Strict);
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Debug_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Debug, totalMessage)).Verifiable();
            _loggerMock.Object.Debug(format, argument);
            _loggerMock.Verify();
        }

        [TestCase]
        public void Debug_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Debug(""));
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
        public void Error_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Error(""));
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
        public void Fatal_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Fatal(""));
#pragma warning restore 1720
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Info_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Info, totalMessage)).Verifiable();
            _loggerMock.Object.Info(format, argument);
            _loggerMock.Verify();
        }

        [TestCase]
        public void Info_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Info(""));
#pragma warning restore 1720
        }

        [TestCase("Hello, {0}!", 12, "Hello, 12!")]
        public void Warn_OutputsFormattedMessage(string format, object argument, string totalMessage)
        {
            _loggerMock.Setup(logger => logger.Log(LogLevel.Warn, totalMessage)).Verifiable();
            _loggerMock.Object.Warn(format, argument);
            _loggerMock.Verify();
        }

        [TestCase]
        public void Warn_LoggerNull_DoesntThrow()
        {
#pragma warning disable 1720 // Expression will always throw a nullreference warning.
            Assert.DoesNotThrow(() => default(Logger).Warn(""));
#pragma warning restore 1720
        }
    }
}
