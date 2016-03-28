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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Smocks.AppDomains;
using Smocks.IL.Filters;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks.IL
{
    internal class AssemblyRewriter : IAssemblyRewriter
    {
        private readonly Configuration _configuration;
        private readonly IMethodRewriter _methodRewriter;
        private readonly IModuleFilter _moduleFilter;
        private readonly List<string> _rewrittenAssemblies = new List<string>();
        private readonly IRewriteTargetCollection _rewriteTargetCollection;

        internal AssemblyRewriter(IMethodRewriter methodRewriter, IModuleFilter moduleFilter)
            : this(new Configuration(), new List<SetupTarget>(), methodRewriter, moduleFilter)
        {
        }

        internal AssemblyRewriter(
            Configuration configuration,
            IRewriteTargetCollection rewriteTargetCollection,
            IMethodRewriter methodRewriter,
            IModuleFilter moduleFilter)
        {
            ArgumentChecker.NotNull(configuration, () => configuration);
            ArgumentChecker.NotNull(rewriteTargetCollection, () => rewriteTargetCollection);
            ArgumentChecker.NotNull(methodRewriter, () => methodRewriter);
            ArgumentChecker.NotNull(moduleFilter, () => moduleFilter);

            _configuration = configuration;
            _rewriteTargetCollection = rewriteTargetCollection;
            _methodRewriter = methodRewriter;
            _moduleFilter = moduleFilter;
        }

        internal AssemblyRewriter(
                Configuration configuration,
                IEnumerable<IRewriteTarget> targets,
                IMethodRewriter methodRewriter,
                IModuleFilter moduleFilter)
            : this(
                configuration,
                new RewriteTargetCollection(targets),
                methodRewriter,
                moduleFilter)
        {
        }

        public void Dispose()
        {
            _moduleFilter.Dispose();

            foreach (string assemblyPath in _rewrittenAssemblies)
            {
                Delete(assemblyPath);
                Delete(Path.ChangeExtension(assemblyPath, ".pdb"));
            }
        }

        public IAssemblyLoader Rewrite(string path)
        {
            bool hasSymbols = File.Exists(Path.ChangeExtension(path, ".pdb"));
            bool rewritten = false;

            ReaderParameters readerParameters = new ReaderParameters { ReadSymbols = hasSymbols };
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path, readerParameters);

            foreach (var module in assembly.Modules)
            {
                if (_moduleFilter.Accepts(module))
                {
                    IRewriteTargetMatcher targetCollection = _rewriteTargetCollection.GetMatcher(module);

                    var types = module.GetTypes().ToList();
                    foreach (var type in types)
                    {
                        rewritten |= ProcessType(_configuration, type, targetCollection);
                    }
                }
            }

            if (!rewritten)
            {
                // No changes made: we can just load the original assembly.
                return new AssemblyLoader(path);
            }

            string outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(path));

            WriterParameters writerParameters = new WriterParameters
            {
                WriteSymbols = hasSymbols
            };

            foreach (var module in assembly.Modules)
            {
                module.Mvid = Guid.NewGuid();                
            }

            assembly.Write(outputPath, writerParameters);

            _rewrittenAssemblies.Add(outputPath);

            return new AssemblyLoader(outputPath);
        }

        private void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }

        private bool ProcessType(Configuration configuration, TypeDefinition type,
            IRewriteTargetMatcher targetMatcher)
        {
            bool rewritten = false;

            foreach (var method in type.Methods)
            {
                rewritten |= _methodRewriter.Rewrite(configuration, method, targetMatcher);
            }

            return rewritten;
        }
    }
}