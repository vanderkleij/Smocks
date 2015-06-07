using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using Smocks.Utility;

namespace Smocks.IL.Dependencies
{
    internal class DependencyGraphNode : IEquatable<DependencyGraphNode>, IDependencyNodeContainer
    {
        private readonly HashSet<DependencyGraphNode> _nodes =
            new HashSet<DependencyGraphNode>();

        private readonly IEqualityComparer<ModuleReference> _moduleComparer;

        public DependencyGraphNode(ModuleReference module,
            IEqualityComparer<ModuleReference> moduleComparer)
        {
            ArgumentChecker.NotNull(module, () => module);
            ArgumentChecker.NotNull(moduleComparer, () => moduleComparer);

            Module = module;
            _moduleComparer = moduleComparer;
        }

        public ISet<DependencyGraphNode> Nodes
        {
            get { return _nodes; }
        }

        public ModuleReference Module { get; private set; }

        public bool Equals(DependencyGraphNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _moduleComparer.Equals(Module, other.Module);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DependencyGraphNode);
        }

        public override int GetHashCode()
        {
            return (Module != null ? Module.GetHashCode() : 0);
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Module.Name;
        }
    }
}