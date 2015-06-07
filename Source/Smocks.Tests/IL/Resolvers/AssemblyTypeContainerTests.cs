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
using NUnit.Framework;
using Smocks.IL.Resolvers;

namespace Smocks.Tests.IL.Resolvers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class AssemblyTypeContainerTests
    {
        [TestCase]
        public void Constructor_AssemblyNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new AssemblyTypeContainer(null));
            Assert.AreEqual("assembly", exception.ParamName);
        }

        [TestCase]
        public void GetType_SlashInsteadOfPlusInNestedTypeName_ReturnsNestedType()
        {
            var subject = new AssemblyTypeContainer(GetType().Assembly);
            var result = subject.GetType(typeof(NestedType).FullName.Replace("+", "/"));

            Assert.AreEqual(typeof(NestedType), result);
        }

        [TestCase]
        public void GetType_TypeInAssembly_ReturnsType()
        {
            var subject = new AssemblyTypeContainer(GetType().Assembly);
            var result = subject.GetType(typeof(AssemblyTypeContainerTests).FullName);

            Assert.AreEqual(typeof(AssemblyTypeContainerTests), result);
        }

        [TestCase]
        public void GetType_TypeNotInAssembly_ThrowsTypeLoadException()
        {
            var subject = new AssemblyTypeContainer(typeof(object).Assembly);

            Assert.Throws<TypeLoadException>(() =>
            {
                subject.GetType(typeof(AssemblyTypeContainerTests).FullName);
            });
        }

        [TestCase]
        public void GetTypes_ReturnsAllTypesInAssembly()
        {
            var assembly = typeof(NestedType).Assembly;
            var subject = new AssemblyTypeContainer(assembly);

            Assert.IsTrue(assembly.GetTypes().SequenceEqual(subject.GetTypes()));
        }

        private class NestedType
        {
        }
    }
}