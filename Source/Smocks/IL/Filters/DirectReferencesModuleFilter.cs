using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Smocks.IL.Dependencies;
using Smocks.Utility;

namespace Smocks.IL.Filters
{
    internal class DirectReferencesModuleFilter : IModuleFilter
    {
        private readonly IDependencyGraph _dependencyGraph;
        private readonly IEqualityComparer<ModuleReference> _moduleComparer;

        internal DirectReferencesModuleFilter(
            IDependencyGraph dependencyGraph,
            IEqualityComparer<ModuleReference> moduleComparer)
        {
            ArgumentChecker.NotNull(dependencyGraph, () => dependencyGraph);
            ArgumentChecker.NotNull(moduleComparer, () => moduleComparer);

            _dependencyGraph = dependencyGraph;
            _moduleComparer = moduleComparer;
        }

        public bool Accepts(ModuleDefinition module)
        {
            return _moduleComparer.Equals(module, _dependencyGraph.Module) || 
                _dependencyGraph.Nodes.Any(node => _moduleComparer.Equals(node.Module, module));
        }
    }
}