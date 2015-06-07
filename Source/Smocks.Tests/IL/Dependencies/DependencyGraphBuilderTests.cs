using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Smocks.IL;
using Smocks.IL.Dependencies;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.IL.Dependencies
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class DependencyGraphBuilderTests
    {
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
                () => new DependencyGraphBuilder(new MethodDisassembler(), null));

            Assert.AreEqual("moduleComparer", exception.ParamName);
        }

        [TestCase]
        public void BuildGraphForMethod_MethodCallAndTokenLoad_ReturnsCorrectDependencies()
        {
            var subject = new DependencyGraphBuilder(new MethodDisassembler(), new ModuleReferenceComparer());
            var result = subject.BuildGraphForMethod(ReflectionUtility.GetLambdaMethod(() =>
            {
                Console.WriteLine(typeof(DependencyGraphBuilderTests));
            }));

            Assert.AreEqual(2, result.Nodes.Count);
        }
    }
}