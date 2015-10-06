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
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Moq;
using NUnit.Framework;
using Smocks.Dummy;

namespace Smocks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class MethodTests
    {
        private static bool _voidMethodInvoked;

        [TestCase]
        public void Setup_Constructor_ReturnsConfiguredValue()
        {
            Smock.Run(context =>
            {
                Uri expected = new Uri("http://www.google.com");

                context.Setup(() => new Uri(It.IsAny<string>())).Returns(expected);
                Assert.AreEqual(expected, new Uri("Invalid"));
            });
        }

        [TestCase]
        public void Setup_Property_ReturnsConfiguredValue()
        {
            Smock.Run(context =>
            {
                DateTime expected = new DateTime(2000, 1, 1);

                context.Setup(() => DateTime.Now).Returns(expected);
                Assert.AreEqual(expected, DateTime.Now);
            });
        }

        [TestCase]
        public void Setup_VoidMethod_DoesntInvokeOriginalWhenMatched()
        {
            Smock.Run(context =>
            {
                context.Setup(() => VoidMethod(It.IsAny<string>())).Verifiable();

                VoidMethod("Foo");

                Assert.AreEqual(false, _voidMethodInvoked);
            });
        }

        [TestCase]
        public void Setup_VoidMethod_InvokesOriginalWhenNotMatched()
        {
            Smock.Run(context =>
            {
                context.Setup(() => VoidMethod("Foo")).Verifiable();

                // Assert pre-conditions
                Assert.AreEqual(false, _voidMethodInvoked);

                VoidMethod("Bar");

                Assert.AreEqual(true, _voidMethodInvoked);
            });
        }

        [TestCase]
        public void Setup_ValueTypeMethodWithArguments_InvokesOriginalMethodWhenNotMatched()
        {
            Smock.Run(context =>
            {
                context.Setup(() => It.IsAny<int>().ToString("NonExistantFormat")).Returns("Test");

                string result = 42.ToString("X");

                Assert.AreEqual("2A", result);
            });
        }

        [TestCase]
        public void Setup_SetupConfiguredFromPrivateMethodInSameClass_ReturnsConfiguredValue()
        {
            Smock.Run(context =>
            {
                DoSetup(context);

                var actual = string.Format(string.Empty);
                Assert.AreEqual("Some value", actual);
            });
        }

        [TestCase]
        public void Setup_SetupIndirectlyConfiguredFromPrivateMethodInSameClass_ReturnsConfiguredValue()
        {
            Smock.Run(context =>
            {
                DoSetupIndirectly(context);

                var actual = string.Format(string.Empty);
                Assert.AreEqual("Some value", actual);
            });
        }

        [TestCase]
        public void Setup_SetupTargetInvokedFromDirectlyReferencedAssembly_IntercepsInvocation()
        {
            Smock.Run(context =>
            {
                context.Setup(() => DateTime.Now).Returns(new DateTime());
                var result = StaticClass.TestDateTime();
                Assert.AreEqual(new DateTime(), result);
            });
        }

        private static void VoidMethod(string arg)
        {
            _voidMethodInvoked = true;
        }

        private static void DoSetup(ISmocksContext context)
        {
            context.Setup(() => string.Format(string.Empty)).Returns("Some value");
        }

        private static void DoSetupIndirectly(ISmocksContext context)
        {
            DoSetup(context);
        }
    }
}