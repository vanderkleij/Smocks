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
        private Mock<IDependencyGraph> _graphMock;
        private Mock<IEqualityComparer<ModuleReference>> _moduleComparerMock;

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

            HashSet<DependencyGraphNode> nodes = new HashSet<DependencyGraphNode>
            {
                new DependencyGraphNode(child, _moduleComparerMock.Object)
            };

            _graphMock.SetupGet(graph => graph.Nodes).Returns(nodes);

            var subject = new DirectReferencesModuleFilter(_graphMock.Object, _moduleComparerMock.Object);
            bool result = subject.Accepts(query);

            Assert.IsTrue(result);
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

        [SetUp]
        public void Setup()
        {
            _moduleComparerMock = new Mock<IEqualityComparer<ModuleReference>>();
            _graphMock = new Mock<IDependencyGraph>();
        }

        private static bool IsAny(ModuleReference module, params ModuleDefinition[] others)
        {
            bool result = module != null && others.Any(other => other.Name == module.Name);
            return result;
        }
    }
}