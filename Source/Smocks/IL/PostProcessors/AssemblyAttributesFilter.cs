using System;
using System.Linq;
using System.Security;
using Mono.Cecil;

namespace Smocks.IL.PostProcessors
{
    internal class AssemblyAttributesFilter : IAssemblyPostProcessor
    {
        private static readonly Type[] AttributesToRemove = 
        {
            typeof(AllowPartiallyTrustedCallersAttribute)
        };

        public void Process(AssemblyDefinition assembly)
        {
            for (int i = assembly.CustomAttributes.Count - 1; i >= 0; --i)
            {
                if (AttributesToRemove.Any(attributeToRemove => assembly.CustomAttributes[i].AttributeType.FullName == attributeToRemove.FullName))
                {
                    assembly.CustomAttributes.RemoveAt(i);
                }
            }
        }
    }
}