using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mono.Cecil;
using Moq;
using NUnit.Framework;
using Smocks.IL;
using Smocks.IL.Dependencies;
using Smocks.IL.Filters;
using Smocks.Tests.TestUtility;

namespace Smocks.Tests.IL.Filters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class DirectReferencesModuleFilterTests
    {
        private Mock<IEqualityComparer<ModuleReference>> _moduleComparerMock;
        private Mock<IDependencyGraph> _graphMock;

        [SetUp]
        public void Setup()
        {
            _moduleComparerMock = new Mock<IEqualityComparer<ModuleReference>>();
            _graphMock = new Mock<IDependencyGraph>();
        }

        [TestCase]
        public void Constructor_DependencyGraphNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DirectReferencesModuleFilter(null, new ModuleReferenceComparer()));

            Assert.AreEqual("dependencyGraph", exception.ParamName);
        }

        [TestCase]
        public void Constructor_ModuleComparerNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new DirectReferencesModuleFilter(_graphMock.Object, null));

            Assert.AreEqual("moduleComparer", exception.ParamName);
        }

        [TestCase]
        public void Accepts_ModuleEqualToDependencyGraphRoot_ReturnsTrue()
        {
            _moduleComparerMock.Setup(comparer => comparer.Equals(It.IsAny<ModuleReference>(),
                It.IsAny<ModuleReference>())).Returns(true);

            var subject = new DirectReferencesModuleFilter(_graphMock.Object, _moduleComparerMock.Object);

            Assert.IsTrue(subject.Accepts(null));
        }

        [TestCase]
        public void Accepts_ModuleUnequalButSomeChildIsEqual_ReturnsTrue()
        {
            var query = CecilUtility.Import(typeof(object)).Resolve().Module;
            var child = CecilUtility.Import(GetType()).Resolve().Module;

            _moduleComparerMock.Setup(comparer => comparer.Equals(child, It.IsAny<ModuleReference>())).Returns(false);

            _moduleComparerMock.Setup(comparer => comparer.Equals(
                    It.Is<ModuleReference>(module => IsAny(module, query, child)),
                    It.Is<ModuleReference>(module => IsAny(module, query, child))))
                .Returns(true);

            HashSet<DependencyGraphNode> nodes = new HashSet<DependencyGraphNode>();
            nodes.Add(new DependencyGraphNode(child, _moduleComparerMock.Object));
            _graphMock.SetupGet(graph => graph.Nodes).Returns(nodes);

            var subject = new DirectReferencesModuleFilter(_graphMock.Object, _moduleComparerMock.Object);
            bool result = subject.Accepts(query);

            Assert.IsTrue(result);
        }

        private static bool IsAny(ModuleReference module, params ModuleDefinition[] others)
        {
            bool result = module != null && others.Any(other => other.Name == module.Name);
            return result;
        }
    }
}