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
using System.IO;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Smocks.AppDomains;
using Smocks.IL;

namespace Smocks.Tests.AppDomains
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class AssemblyLoaderFactoryTests
    {
        private Mock<IAssemblyRewriter> _assemblyRewriterMock;

        [TestCase]
        public void Dispose_DisposesAssemblyRewriter()
        {
            var subject = new AssemblyLoaderFactory(Path.GetTempPath(), _assemblyRewriterMock.Object);
            subject.Dispose();

            _assemblyRewriterMock.Verify(rewriter => rewriter.Dispose(), Times.Once);
        }

        [TestCase]
        public void GetLoaderForAssembly_ExistingAssembly_ReturnsRewriterResult()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            var loader = new AssemblyLoader(location);

            _assemblyRewriterMock
                .Setup(rewriter => rewriter.Rewrite(location))
                .Returns(loader);

            var subject = new AssemblyLoaderFactory(Path.GetDirectoryName(location), _assemblyRewriterMock.Object);

            // Act
            var result = subject.GetLoaderForAssembly(Assembly.GetExecutingAssembly().GetName());

            // Assert
            Assert.AreEqual(loader, result);
            _assemblyRewriterMock.Verify();
        }

        [TestCase]
        public void GetLoaderForAssembly_NonexistingAssembly_ReturnsNull()
        {
            var subject = new AssemblyLoaderFactory(Path.GetTempPath(), _assemblyRewriterMock.Object);
            var result = subject.GetLoaderForAssembly(new AssemblyName(Guid.NewGuid().ToString()));

            Assert.IsNull(result);
        }

        [TestCase]
        public void RegisterAssembly_GetLoaderForAssemblyLoadsFromSuppliedPath()
        {
            string assemblyName = Guid.NewGuid().ToString();
            string path = Path.Combine(@"c:\temp", assemblyName + ".dll");

            var subject = new AssemblyLoaderFactory(Path.GetTempPath(), _assemblyRewriterMock.Object);
            subject.RegisterAssembly(assemblyName, path);

            var loader = new AssemblyLoader(path);
            _assemblyRewriterMock
                .Setup(rewriter => rewriter.Rewrite(path))
                .Returns(loader)
                .Verifiable();

            // Act
            var result = subject.GetLoaderForAssembly(new AssemblyName(assemblyName));

            // Assert
            Assert.AreEqual(loader, result);
            _assemblyRewriterMock.Verify();
        }

        [SetUp]
        public void Setup()
        {
            _assemblyRewriterMock = new Mock<IAssemblyRewriter>();
        }
    }
}