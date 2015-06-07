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
using Smocks.IL.Resolvers;
using Smocks.Logging;

namespace Smocks.Utility
{
    /// <summary>
    /// A class for discovering types that implement some interface
    /// in a given <see cref="ITypeContainer"/>. TODO: use the basic
    /// dependency injection framework instead.
    /// </summary>
    internal class Discoverer
    {
        private readonly Logger _logger;
        private readonly ITypeContainer _typeContainer;

        public Discoverer(ITypeContainer typeContainer)
            : this(typeContainer, null)
        {
        }

        public Discoverer(ITypeContainer typeContainer, Logger logger)
        {
            ArgumentChecker.NotNull(typeContainer, () => typeContainer);

            _typeContainer = typeContainer;
            _logger = logger;
        }

        public IEnumerable<T> GetAll<T>()
        {
            foreach (var type in _typeContainer.GetTypes())
            {
                if (typeof(T).IsAssignableFrom(type) &&
                    type.IsClass &&
                    !type.IsAbstract)
                {
                    T instance;

                    try
                    {
                        instance = (T)Activator.CreateInstance(type);
                    }
                    catch (Exception)
                    {
                        _logger.Warn("Could not create instance of {0}", typeof(T).FullName);
                        continue;
                    }

                    yield return instance;
                }
            }
        }
    }
}