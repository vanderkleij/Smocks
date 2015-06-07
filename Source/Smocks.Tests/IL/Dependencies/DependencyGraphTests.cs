using System;
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using NUnit.Framework;
using Smocks.IL;
using Smocks.IL.Dependencies;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.IL.Dependencies
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class DependencyGraphTests
    {
        [TestCase]
        public void Constructor_ModuleNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DependencyGraph(null, new ModuleReferenceComparer()));

            Assert.AreEqual("module", exception.ParamName);
        }

        [TestCase]
        public void Constructor_ModuleComparerNull_ThrowsArgumentNullException()
        {
            var module = CecilUtility.Import(typeof(object)).Resolve().Module;
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DependencyGraph(module, null));

            Assert.AreEqual("moduleComparer", exception.ParamName);
        }
    }
}