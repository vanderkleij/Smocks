using System.Collections.Generic;
using Mono.Cecil;

namespace Smocks.IL
{
    internal class ModuleReferenceComparer : IEqualityComparer<ModuleReference>
    {
        public bool Equals(ModuleReference left, ModuleReference right)
        {
            return string.Equals(left.Name, right.Name);
        }

        public int GetHashCode(ModuleReference reference)
        {
            return reference.Name.GetHashCode();
        }
    }
}