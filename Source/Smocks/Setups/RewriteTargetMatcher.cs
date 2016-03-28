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
using System.Collections.ObjectModel;
using System.Linq;
using Mono.Cecil;
using Smocks.IL;
using Smocks.Utility;

namespace Smocks.Setups
{
    internal class RewriteTargetMatcher : IRewriteTargetMatcher
    {
        private readonly List<Tuple<IRewriteTarget, MethodReference>> _targets;

        public RewriteTargetMatcher(IMethodImporter methodImporter, ReadOnlyCollection<IRewriteTarget> targets)
        {
            ArgumentChecker.NotNull(methodImporter, () => methodImporter);
            ArgumentChecker.NotNull(targets, () => targets);

            _targets = targets
                .SelectMany(target => target.Methods, (target, method) => Tuple.Create(target, methodImporter.Import(method)))
                .ToList();
        }

        public IEnumerable<IRewriteTarget> GetMatchingTargets(MethodReference method)
        {
            return _targets
                .Where(pair => pair.Item2.FullName.Equals(method.FullName))
                .Select(pair => pair.Item1);
        }
    }
}