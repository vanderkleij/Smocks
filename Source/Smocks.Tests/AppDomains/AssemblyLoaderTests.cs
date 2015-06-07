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
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Smocks.AppDomains;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.AppDomains
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class AssemblyLoaderTests
    {
        [TestCase(default(string))]
        [TestCase("")]
        [TestCase(@"c:\temp")]
        public void Constructor_SetsPathProperty(string path)
        {
            var subject = new AssemblyLoader(path);
            Assert.AreEqual(path, subject.Path);
        }

        [TestCase(default(string))]
        [TestCase("")]
        public void Load_NullOrEmptyPath_ReturnsNull(string path)
        {
            var subject = new AssemblyLoader(path);
            var result = subject.Load();

            Assert.AreEqual(null, result);
        }

        [TestCase]
        public void Load_ValidAssemblyPath_LoadsAssemblyFromPath()
        {
            // TODO: check if we can use Smocks to unit test Smocks itself.
            // Currently we cannot mock Assembly.LoadFrom so we resort
            // to actually loading an assembly in a temporary appdomain.
            string assemblyName = Guid.NewGuid().ToString();
            string dummyAssemblyPath = AssemblyUtility.CreateDummyAssembly(new AssemblyName(assemblyName));

            AppDomainUtility.InvokeInOtherAppDomain(dummyAssemblyPath, assemblyName, (path, name) =>
            {
                // Act
                var subject = new AssemblyLoader(path);
                var result = subject.Load();

                // Assert
                Assert.NotNull(result);
                Assert.AreEqual(name, result.GetName().Name);
                Assert.AreEqual(path, result.Location);
            });

            // Cleanup
            File.Delete(dummyAssemblyPath);
        }
    }
}