using Mono.Cecil;

namespace Smocks.IL
{
    internal interface IAssemblyPostProcessor
    {
        void Process(AssemblyDefinition assembly);
    }
}