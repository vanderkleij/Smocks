using System.Reflection;
using Mono.Cecil;

namespace Smocks.IL.Filters
{
    internal interface IModuleFilter
    {
        bool Accepts(ModuleDefinition module);
    }
}