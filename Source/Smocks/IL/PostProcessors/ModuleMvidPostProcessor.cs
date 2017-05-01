using System;
using Mono.Cecil;

namespace Smocks.IL.PostProcessors
{
    internal class ModuleMvidPostProcessor : IAssemblyPostProcessor
    {
        public void Process(AssemblyDefinition assembly)
        {
            foreach (var module in assembly.Modules)
            {
                module.Mvid = Guid.NewGuid();
            }
        }
    }
}