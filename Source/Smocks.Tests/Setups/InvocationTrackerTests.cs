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
using System.Linq;
using Moq;
using NUnit.Framework;
using Smocks.Exceptions;
using Smocks.Setups;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.Setups
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class InvocationTrackerTests
    {
        private Mock<ISetupManager> _setupManagerMock;

        [TestCase]
        public void Constructor_SetupManagerNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new InvocationTracker(null));
            Assert.AreEqual("setupManager", exception.ParamName);
        }

        [SetUp]
        public void Setup()
        {
            _setupManagerMock = new Mock<ISetupManager>();
            ConfigureSetups();
        }

        [TestCase]
        public void Verify_NoSetupsOrInvocations_DoesntThrow()
        {
            var subject = new InvocationTracker(_setupManagerMock.Object);
            Assert.DoesNotThrow(() => subject.Verify());
        }

        [TestCase]
        public void Verify_NoVerifiableSetupsOrInvocations_DoesntThrow()
        {
            // We have a setup, but it's not verifiable
            var setup = new Setup(TestDataFactory.CreateMethodCallInfo());
            ConfigureSetups(setup);

            var subject = new InvocationTracker(_setupManagerMock.Object);
            Assert.DoesNotThrow(() => subject.Verify());
        }

        [TestCase]
        public void Verify_VerifiableSetupsButNoInvocations_ThrowsVerificationException()
        {
            var setup = new Setup(TestDataFactory.CreateMethodCallInfo());
            setup.Verifiable();
            ConfigureSetups(setup);

            var subject = new InvocationTracker(_setupManagerMock.Object);
            Assert.Throws<VerificationException>(() => subject.Verify());
        }

        [TestCase]
        public void Verify_VerifiableSetupsButNoMatchingInvocations_ThrowsVerificationException()
        {
            var setup = new Setup(TestDataFactory.CreateMethodCallInfo(() => Console.WriteLine()));
            setup.Verifiable();
            ConfigureSetups(setup);

            var subject = new InvocationTracker(_setupManagerMock.Object);

            subject.Track(ReflectionUtility.GetMethod(() => Console.ReadKey()), new object[0], null);

            Assert.Throws<VerificationException>(() => subject.Verify());
        }

        [TestCase]
        public void Verify_VerifiableSetupsWithMatchingInvocations_DoesntThrow()
        {
            var setup = new Setup(TestDataFactory.CreateMethodCallInfo(() => Console.WriteLine()));
            setup.Verifiable();
            ConfigureSetups(setup);

            var subject = new InvocationTracker(_setupManagerMock.Object);

            subject.Track(ReflectionUtility.GetMethod(() => Console.WriteLine()), new object[0], setup);

            Assert.DoesNotThrow(() => subject.Verify());
        }

        private void ConfigureSetups(params IInternalSetup[] setups)
        {
            _setupManagerMock
                .Setup(manager => manager.GetEnumerator())
                .Returns(setups.ToList().GetEnumerator());
        }
    }
}