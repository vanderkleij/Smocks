using Smocks.IL.Dependencies;

namespace Smocks.IL.Filters
{
    internal interface IModuleFilterFactory
    {
        IModuleFilter GetFilter(Scope scope, DependencyGraph dependencyGraph);
    }
}