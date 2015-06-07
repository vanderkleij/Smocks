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
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Smocks.Utility;

namespace Smocks.IL.Resolvers
{
    internal class MethodResolver : IMethodResolver
    {
        private readonly ITypeResolver _typeResolver;

        internal MethodResolver(ITypeResolver typeResolver)
        {
            ArgumentChecker.NotNull(typeResolver, () => typeResolver);

            _typeResolver = typeResolver;
        }

        public MethodBase Resolve(MethodReference methodReference)
        {
            return Resolve(methodReference, GenericBindingContext.Create(methodReference));
        }

        public MethodBase Resolve(MethodReference methodReference, GenericBindingContext bindingContext)
        {
            var methodDefinition = methodReference.Resolve();

            Type declaringType = _typeResolver.Resolve(methodDefinition.DeclaringType);

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.ExactBinding;
            flags |= methodDefinition.HasThis ? BindingFlags.Instance : BindingFlags.Static;

            IEnumerable<MethodBase> candidates = IsConstructor(methodDefinition.Name)
                ? (IEnumerable<MethodBase>)declaringType.GetConstructors(flags)
                : declaringType.GetMethods(flags);

            MethodBase result;

            if (methodReference.IsGenericInstance)
            {
                MethodBase genericMethod = Resolve(methodDefinition, bindingContext);

                result = BindGenericArguments(methodReference, genericMethod);
            }
            else
            {
                TypeReference[] parameterTypes = methodDefinition.Parameters.Select(parameter =>
                    parameter.ParameterType).ToArray();

                result = FindMethod(candidates, methodDefinition.Name, parameterTypes,
                    methodDefinition.HasGenericParameters, bindingContext);
            }

            return result;
        }

        private MethodBase BindGenericArguments(MethodReference methodReference, MethodBase definition)
        {
            GenericInstanceMethod genericMethod = methodReference as GenericInstanceMethod;
            if (genericMethod != null)
            {
                Type[] genericArguments = genericMethod.GenericArguments.Select(argument =>
                    _typeResolver.Resolve(argument)).ToArray();
                definition = ((MethodInfo)definition).MakeGenericMethod(genericArguments);
            }

            return definition;
        }

        private MethodBase FindMethod(IEnumerable<MethodBase> candidates, string name,
            TypeReference[] parameterTypes, bool isGeneric, GenericBindingContext bindingContext)
        {
            var result = candidates.Where(method => method.Name == name);
            result = result.Where(method => method.IsGenericMethod == isGeneric);
            result = result.Where(method => ParametersMatch(method, parameterTypes, bindingContext));

            return result.Single();
        }

        private bool IsConstructor(string name)
        {
            return name == ".ctor";
        }

        private bool ParametersMatch(MethodBase method, TypeReference[] parameterTypes,
            GenericBindingContext bindingContext)
        {
            var methodParameters = method.GetParameters().Select(parameter => parameter.ParameterType).ToList();

            if (methodParameters.Count != parameterTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < methodParameters.Count; ++i)
            {
                Type methodParameter = methodParameters[i];
                TypeReference otherParameter = parameterTypes[i];

                if (methodParameter.ContainsGenericParameters)
                {
                    var otherParameterType = _typeResolver.Resolve(otherParameter.Resolve());
                    var methodParameterGeneric = methodParameter.GetGenericTypeDefinition();

                    if (methodParameterGeneric != otherParameterType)
                    {
                        return false;
                    }
                }
                else
                {
                    if (methodParameter != _typeResolver.Resolve(otherParameter, bindingContext))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}