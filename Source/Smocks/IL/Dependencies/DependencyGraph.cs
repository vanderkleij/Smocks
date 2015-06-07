using System.Collections.Generic;
using Mono.Cecil;

namespace Smocks.IL.Dependencies
{
    internal class DependencyGraph : DependencyGraphNode, IDependencyGraph
    {
        public DependencyGraph(
                ModuleReference module, 
                IEqualityComparer<ModuleReference> moduleComparer)
            : base(module, moduleComparer)
        {
        }
    }
}