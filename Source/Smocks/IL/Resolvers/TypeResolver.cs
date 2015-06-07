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
using System.Linq;
using Mono.Cecil;
using Smocks.Utility;

namespace Smocks.IL.Resolvers
{
    internal class TypeResolver : ITypeResolver
    {
        private readonly IModuleResolver _moduleResolver;

        internal TypeResolver(IModuleResolver moduleResolver)
        {
            ArgumentChecker.NotNull(moduleResolver, () => moduleResolver);

            _moduleResolver = moduleResolver;
        }

        public Type Resolve(TypeReference type)
        {
            return Resolve(type, new GenericBindingContext());
        }

        public Type Resolve(TypeReference type, GenericBindingContext bindingContext)
        {
            ITypeContainer container = _moduleResolver.Resolve(type.Scope);

            GenericInstanceType genericInstanceType = type as GenericInstanceType;
            if (genericInstanceType != null)
            {
                Type genericType = Resolve(genericInstanceType.Resolve());
                Type[] genericArguments = genericInstanceType.GenericArguments
                    .Select(argument => bindingContext.Resolve(argument))
                    .Select(argument => Resolve(argument, bindingContext)).ToArray();
                return genericType.MakeGenericType(genericArguments);
            }

            return container.GetType(type.FullName);
        }
    }
}