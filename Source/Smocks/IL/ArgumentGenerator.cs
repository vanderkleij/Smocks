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
using Smocks.Injection;
using Smocks.Utility;

namespace Smocks.IL
{
    internal class ArgumentGenerator : IArgumentGenerator
    {
        private readonly IServiceCreator _serviceCreator;
        private readonly IServiceLocator _serviceLocator;

        internal ArgumentGenerator(IServiceLocator serviceLocator,
            IServiceCreator serviceCreator)
        {
            ArgumentChecker.NotNull(serviceLocator, () => serviceLocator);
            ArgumentChecker.NotNull(serviceCreator, () => serviceCreator);

            _serviceLocator = serviceLocator;
            _serviceCreator = serviceCreator;
        }

        public IEnumerable<object> GetArguments(Type[] parameterTypes, object target)
        {
            for (int i = 0; i < parameterTypes.Length; ++i)
            {
                if (i == 0 && target != null)
                {
                    if (parameterTypes[0].IsInstanceOfType(target) == false)
                    {
                        throw new ArgumentException("Target not assignable to first parameter", "target");
                    }

                    yield return target;
                }
                else
                {
                    object argument = _serviceCreator.Create(parameterTypes[i], _serviceLocator.Container);
                    if (argument != null)
                    {
                        yield return argument;
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not create argument of type "
                            + parameterTypes[i].FullName);
                    }
                }
            }
        }
    }
}