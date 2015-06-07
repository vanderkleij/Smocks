using System;
using System.Collections.Generic;
using Mono.Cecil;
using Smocks.IL.Dependencies;
using Smocks.Utility;

namespace Smocks.IL.Filters
{
    internal class ModuleFilterFactory : IModuleFilterFactory
    {
        private readonly IEqualityComparer<ModuleReference> _moduleComparer;

        internal ModuleFilterFactory(IEqualityComparer<ModuleReference> moduleComparer)
        {
            ArgumentChecker.NotNull(moduleComparer, () => moduleComparer);

            _moduleComparer = moduleComparer;
        }

        public IModuleFilter GetFilter(Scope scope, DependencyGraph dependencyGraph)
        {
            switch (scope)
            {
                case Scope.All:
                    return new AllowAllModuleFilter();

                case Scope.DirectReferences:
                    return new DirectReferencesModuleFilter(dependencyGraph, _moduleComparer);

                default:
                    throw new NotSupportedException("Unknown scope");
            }
        }
    }
}