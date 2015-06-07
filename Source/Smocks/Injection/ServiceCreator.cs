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
using Smocks.Utility;

namespace Smocks.Injection
{
    internal class ServiceCreator : IServiceCreator
    {
        public object Create(Type type, IServiceLocatorContainer container)
        {
            ArgumentChecker.NotNull(type, () => type);

            // Prefer constructors with more parameters
            var constructors = type
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderByDescending(constructor => constructor.GetParameters().Length)
                .ToList();

            foreach (var constructor in constructors)
            {
                object[] arguments = ResolveArguments(container,
                    constructor.GetParameters().Select(parameter => parameter.ParameterType));

                if (arguments != null)
                {
                    return constructor.Invoke(arguments);
                }
            }

            throw new InvalidOperationException("No suitable constructor found for " + type.Name);
        }

        private object[] ResolveArguments(IServiceLocatorContainer container, IEnumerable<Type> parameterTypes)
        {
            List<object> arguments = new List<object>();

            foreach (Type parameterType in parameterTypes)
            {
                object argument;

                if (container.TryResolve(parameterType, out argument))
                {
                    arguments.Add(argument);
                }
                else
                {
                    return null;
                }
            }

            return arguments.ToArray();
        }
    }
}