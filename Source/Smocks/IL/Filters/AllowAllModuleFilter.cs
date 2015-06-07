using Mono.Cecil;

namespace Smocks.IL.Filters
{
    internal class AllowAllModuleFilter : IModuleFilter
    {
        public bool Accepts(ModuleDefinition module)
        {
            return true;
        }
    }
}