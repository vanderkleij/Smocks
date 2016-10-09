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
using System.Threading;
using Smocks.Utility;

namespace Smocks.Injection
{
    internal class ServiceLocator : IServiceLocator
    {
        public static IServiceLocator Default = new ServiceLocator(
            new ServiceLocatorContainer(new DefaultServiceLocatorSetup()));

        private static IServiceLocator _instance;

        private readonly IServiceLocatorContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        internal ServiceLocator(IServiceLocatorContainer container)
        {
            ArgumentChecker.NotNull(container, () => container);

            // Register self so that IServiceLocator can be resolved.
            container.RegisterSingleton<IServiceLocator, ServiceLocator>(this);

            _container = container;
        }

        public static IServiceLocator Instance
        {
            get
            {
                return _instance;
            }

            set
            {
                var original = Interlocked.CompareExchange(ref _instance, value, null);

                if (original != null)
                {
                    throw new InvalidOperationException("Instance can only be set once");
                }
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        public IServiceLocatorContainer Container
        {
            get { return _container; }
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }
    }
}