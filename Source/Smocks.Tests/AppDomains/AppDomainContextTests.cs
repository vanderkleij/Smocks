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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Moq;
using NUnit.Framework;
using Smocks.AppDomains;
using Smocks.IL;
using Smocks.IL.Filters;

namespace Smocks.Tests.AppDomains
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class AppDomainContextTests
    {
        private Mock<IMethodRewriter> _methodRewriterMock;
        private Mock<IModuleFilter> _moduleFilterMock;

        [TestCase]
        public void InvokeAction_RunsInOtherAppDomain()
        {
            int otherAppDomainId = AppDomain.CurrentDomain.Id;

            var subject = new AppDomainContext(
                new AssemblyRewriter(_methodRewriterMock.Object, _moduleFilterMock.Object));
            subject.Invoke(() =>
            {
                otherAppDomainId = AppDomain.CurrentDomain.Id;
            });

            Assert.AreNotEqual(AppDomain.CurrentDomain.Id, otherAppDomainId);
        }

        [TestCase]
        public void InvokeFunc_EnumerableReturnValue_TransportsEnumerable()
        {
            var subject = new AppDomainContext(
                new AssemblyRewriter(_methodRewriterMock.Object, _moduleFilterMock.Object));
            IEnumerable range = subject.Invoke(() => (IEnumerable)Enumerable.Range(1, 100));

            Assert.AreEqual(100, range.Cast<int>().Count());
        }

        [TestCase]
        public void InvokeFunc_GenericEnumerableReturnValue_TransportsEnumerable()
        {
            var subject = new AppDomainContext(
                new AssemblyRewriter(_methodRewriterMock.Object, _moduleFilterMock.Object));
            IEnumerable<int> range = subject.Invoke(() => Enumerable.Range(1, 100));

            Assert.AreEqual(100, range.Count());
        }

        [TestCase]
        public void InvokeFunc_RunsInOtherAppDomain()
        {
            var subject = new AppDomainContext(
                new AssemblyRewriter(_methodRewriterMock.Object, _moduleFilterMock.Object));
            var otherAppDomainId = subject.Invoke(() => AppDomain.CurrentDomain.Id);

            Assert.AreNotEqual(AppDomain.CurrentDomain.Id, otherAppDomainId);
        }

        [TestCase]
        public void RegisterForDeletion_Dispose_DeletesProvidedPath()
        {
            string path = Path.GetTempFileName();

            // Assert pre-conditions
            Assert.IsTrue(File.Exists(path));

            // Act
            using (var subject = new AppDomainContext(
                new AssemblyRewriter(_methodRewriterMock.Object, _moduleFilterMock.Object)))
            {
                subject.RegisterForDeletion(path);
            }

            // Assert post-conditions
            Assert.IsFalse(File.Exists(path));
        }

        [SetUp]
        public void Setup()
        {
            _methodRewriterMock = new Mock<IMethodRewriter>();
            _moduleFilterMock = new Mock<IModuleFilter>();
            _moduleFilterMock.Setup(filter => filter.Accepts(It.IsAny<ModuleDefinition>())).Returns(true);
        }
    }
}