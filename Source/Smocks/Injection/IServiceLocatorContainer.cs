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

namespace Smocks.Injection
{
    /// <summary>
    /// A dependency injection container. Used to register and subsequently
    /// resolve types.
    /// </summary>
    public interface IServiceLocatorContainer
    {
        /// <summary>
        /// Registers an implementation type for a service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        void Register<TService, TImplementation>()
            where TImplementation : TService;

        /// <summary>
        /// Registers an implementation type for a service type as a singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        void RegisterSingleton<TService, TImplementation>()
            where TImplementation : TService;

        /// <summary>
        /// Registers an instance of an implementation type for a service type as a singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        void RegisterSingleton<TService, TImplementation>(TImplementation instance)
            where TImplementation : TService;

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <returns>And instance of <see cref="T"/>.</returns>
        T Resolve<T>();

        /// <summary>
        /// Attempts to resolve the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance, if one could be resolved.</param>
        /// <returns>Whether an instance could be resolved.</returns>
        bool TryResolve(Type type, out object instance);

        /// <summary>
        /// Returns an instance of every registered implementation of the specified type.
        /// </summary>
        /// <typeparam name="TService">The type to resolve.</typeparam>
        /// <returns>An instance of every registered implementation of the specified type.</returns>
        IEnumerable<TService> ResolveAll<TService>();
    }
}