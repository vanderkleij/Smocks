using System.Collections.Generic;

namespace Smocks.IL.Dependencies
{
    internal interface IDependencyNodeContainer
    {
        ISet<DependencyGraphNode> Nodes { get; }
    }
}