#region License
//// The MIT License (MIT)
////
//// Copyright (c) 2015 Tom van der Kleij
////
//// Permission is hereby granted, free of charge, to any person obtaining a copy of
//// this software and associated documentation files (the "Software"), to deal in
//// the Software without restriction, including without limitation the rights to
//// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//// the Software, and to permit persons to whom the Software is furnished to do so,
//// subject to the following conditions:
////
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
////
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion License

using System;
using System.Reflection;

namespace Smocks.AppDomains
{
    /// <summary>
    /// Responsible for resolving dependencies. This class subscribes to the
    /// <see cref="AppDomain.AssemblyResolve"/> event and forwards requests
    /// for loading assemblies to the provided <see cref="IAssemblyLoaderFactory"/>.
    /// </summary>
    public class AssemblyResolver : MarshalByRefObject
    {
        private readonly ILoadedAssemblyFinder _loadedAssemblyFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
        /// </summary>
        public AssemblyResolver()
        {
            _loadedAssemblyFinder = new LoadedAssemblyFinder();
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyResolver" /> class.
        /// </summary>
        /// <param name="loadedAssemblyFinder">The loaded assembly finder.</param>
        /// <param name="loaderFactory">The factory.</param>
        internal AssemblyResolver(ILoadedAssemblyFinder loadedAssemblyFinder,
            IAssemblyLoaderFactory loaderFactory)
        {
            _loadedAssemblyFinder = loadedAssemblyFinder;
            AssemblyLoaderFactory = loaderFactory;
        }

        /// <summary>
        /// Gets or sets the <see cref="IAssemblyLoaderFactory"/> that is used to
        /// locate the assembly to load.
        /// </summary>
        public IAssemblyLoaderFactory AssemblyLoaderFactory { get; set; }

        /// <summary>
        /// Resolves assemblies by loading an <see cref="Assembly"/> for a given
        /// <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ResolveEventArgs"/> instance containing
        /// the <see cref="AssemblyName"/>.</param>
        /// <returns>An assembly if one could be loaded, or null.</returns>
        internal Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Temporarily disable resolving to prevent infinite recursion.
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;

            try
            {
                AssemblyName assemblyName = new AssemblyName(args.Name);

                Assembly alreadyLoadedAssembly = _loadedAssemblyFinder.Find(assemblyName);
                if (alreadyLoadedAssembly != null)
                {
                    return alreadyLoadedAssembly;
                }

                if (AssemblyLoaderFactory != null)
                {
                    IAssemblyLoader loader = AssemblyLoaderFactory.GetLoaderForAssembly(assemblyName);
                    Assembly loadedAssembly = loader != null ? loader.Load() : null;

                    return loadedAssembly;
                }

                return null;
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            }
        }

        /// <summary>
        /// Called when the appdomain is unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDomainUnload(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            AppDomain.CurrentDomain.DomainUnload -= OnDomainUnload;
        }
    }
}