using System.Reflection;

namespace Smocks.IL.Dependencies
{
    internal interface IDependencyGraphBuilder
    {
        DependencyGraph BuildGraphForMethod(MethodBase method);
    }
}