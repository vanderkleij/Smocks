using System.Collections.Generic;
using Mono.Cecil;

namespace Smocks.IL.Dependencies
{
    internal interface IDependencyGraph
    {
        ISet<DependencyGraphNode> Nodes { get; }
        ModuleReference Module { get; }
    }
}