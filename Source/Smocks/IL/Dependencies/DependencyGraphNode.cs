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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mono.Cecil;
using Smocks.Utility;

namespace Smocks.IL.Dependencies
{
    internal class DependencyGraphNode : IEquatable<DependencyGraphNode>, IDependencyNodeContainer
    {
        private readonly IEqualityComparer<ModuleReference> _moduleComparer;

        private readonly HashSet<DependencyGraphNode> _nodes =
            new HashSet<DependencyGraphNode>();

        public DependencyGraphNode(ModuleReference module,
            IEqualityComparer<ModuleReference> moduleComparer)
        {
            ArgumentChecker.NotNull(module, () => module);
            ArgumentChecker.NotNull(moduleComparer, () => moduleComparer);

            Module = module;
            _moduleComparer = moduleComparer;
        }

        public ModuleReference Module { get; private set; }

        public ISet<DependencyGraphNode> Nodes
        {
            get { return _nodes; }
        }

        public bool Equals(DependencyGraphNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _moduleComparer.Equals(Module, other.Module);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DependencyGraphNode);
        }

        public override int GetHashCode()
        {
            return Module != null ? Module.GetHashCode() : 0;
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Module.Name;
        }
    }
}