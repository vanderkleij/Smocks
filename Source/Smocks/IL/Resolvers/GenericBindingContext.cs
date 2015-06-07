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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mono.Cecil;

namespace Smocks.IL.Resolvers
{
    internal class GenericBindingContext
    {
        private static readonly Regex MethodIndexPattern = new Regex(@"^!!(\d+)$", RegexOptions.Compiled);
        private static readonly Regex TypeIndexPattern = new Regex(@"^!(\d+)$", RegexOptions.Compiled);
        private readonly List<Entry> _methodEntries = new List<Entry>();
        private readonly List<Entry> _typeEntries = new List<Entry>();

        public GenericBindingContext()
        {
        }

        private GenericBindingContext(GenericInstanceMethod method)
        {
            var definition = method.Resolve();

            for (int i = 0; i < definition.GenericParameters.Count; ++i)
            {
                var parameter = definition.GenericParameters[i];
                var binding = method.GenericArguments[i];
                _methodEntries.Add(new Entry(parameter.FullName, binding));
            }
        }

        public static GenericBindingContext Create(MethodReference methodReference)
        {
            var genericMethod = methodReference as GenericInstanceMethod;
            return genericMethod != null
                ? new GenericBindingContext(genericMethod)
                : new GenericBindingContext();
        }

        public TypeReference Resolve(TypeReference argument)
        {
            Match match = TypeIndexPattern.Match(argument.FullName);

            if (match.Success)
            {
                return _typeEntries[int.Parse(match.Groups[1].Value)].Binding;
            }

            match = MethodIndexPattern.Match(argument.FullName);

            if (match.Success)
            {
                return _methodEntries[int.Parse(match.Groups[1].Value)].Binding;
            }

            var methodEntry = _methodEntries.FirstOrDefault(entry => entry.Name == argument.FullName);

            return methodEntry != null ? methodEntry.Binding : argument;
        }

        private class Entry
        {
            public Entry(string name, TypeReference binding)
            {
                Name = name;
                Binding = binding;
            }

            public TypeReference Binding { get; private set; }

            public string Name { get; private set; }
        }
    }
}