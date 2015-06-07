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
#endregion

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using Smocks.IL;
using Smocks.Utility;

namespace Smocks.AppDomains
{
    internal class AssemblyLoaderFactory : MarshalByRefObject, IAssemblyLoaderFactory
    {
        private static readonly string[] AssemblyExtensions = { ".dll", ".exe" };

        private readonly IAssemblyRewriter _assemblyRewriter;
        private readonly string _basePath;

        private readonly ConcurrentDictionary<string, string> _knownAssemblies =
            new ConcurrentDictionary<string, string>();

        public AssemblyLoaderFactory(string basePath, IAssemblyRewriter assemblyRewriter)
        {
            ArgumentChecker.NotNull(assemblyRewriter, () => assemblyRewriter);

            _basePath = basePath;
            _assemblyRewriter = assemblyRewriter;
        }

        public void Dispose()
        {
            _assemblyRewriter.Dispose();
        }

        public IAssemblyLoader GetLoaderForAssembly(AssemblyName assemblyName)
        {
            string knownAssemblyPath;

            if (_knownAssemblies.TryGetValue(assemblyName.FullName, out knownAssemblyPath))
            {
                return _assemblyRewriter.Rewrite(knownAssemblyPath);
            }

            foreach (string extension in AssemblyExtensions)
            {
                string path = Path.Combine(_basePath, assemblyName.Name + extension);

                if (File.Exists(path))
                {
                    return _assemblyRewriter.Rewrite(path);
                }
            }

            return null;
        }

        public void RegisterAssembly(string fullName, string assemblyLocation)
        {
            _knownAssemblies.TryAdd(fullName, assemblyLocation);
        }
    }
}