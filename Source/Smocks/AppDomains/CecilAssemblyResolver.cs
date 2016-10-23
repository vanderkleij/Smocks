using System;
using Mono.Cecil;
using Smocks.Utility;

namespace Smocks.AppDomains
{
    public class CecilAssemblyResolver : DefaultAssemblyResolver
    {
        public CecilAssemblyResolver(Configuration configuration)
        {
            ArgumentChecker.NotNull(configuration, nameof(configuration));

            AddSearchDirectory(AppDomain.CurrentDomain.BaseDirectory);

            if (configuration.AssemblySearchDirectories != null)
            {
                foreach (string searchDirectory in configuration.AssemblySearchDirectories)
                {
                    AddSearchDirectory(searchDirectory);
                }
            }
        }

        public CecilAssemblyResolver()
            : this(new Configuration())
        {
        }
    }
}