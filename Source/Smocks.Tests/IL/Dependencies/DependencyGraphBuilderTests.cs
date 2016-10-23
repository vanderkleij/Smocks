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
using NUnit.Framework;
using Smocks.AppDomains;
using Smocks.IL;
using Smocks.IL.Dependencies;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.IL.Dependencies
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class DependencyGraphBuilderTests
    {
        private static readonly MethodDisassembler Disassembler = new MethodDisassembler(new CecilAssemblyResolver());

        [TestCase]
        public void BuildGraphForMethod_MethodCallAndTokenLoad_ReturnsCorrectDependencies()
        {
            var subject = new DependencyGraphBuilder(Disassembler, new ModuleReferenceComparer());
            var result = subject.BuildGraphForMethod(ReflectionUtility.GetLambdaMethod(() =>
            {
                Console.WriteLine(typeof(DependencyGraphBuilderTests));
            }));

            Assert.AreEqual(2, result.Nodes.Count);
        }

        [TestCase]
        public void Constructor_DisassemblerNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DependencyGraphBuilder(null, new ModuleReferenceComparer()));

            Assert.AreEqual("disassembler", exception.ParamName);
        }

        [TestCase]
        public void Constructor_ModuleComparerNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DependencyGraphBuilder(Disassembler, null));

            Assert.AreEqual("moduleComparer", exception.ParamName);
        }
    }
}