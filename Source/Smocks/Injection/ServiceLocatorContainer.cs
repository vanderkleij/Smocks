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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Smocks.Utility;

namespace Smocks.Injection
{
    internal class ServiceLocatorContainer : IServiceLocatorContainer
    {
        private readonly ConcurrentDictionary<Type, Delegate[]> _registrations = 
            new ConcurrentDictionary<Type, Delegate[]>();

        private readonly IServiceCreator _serviceCreator;

        private readonly ConcurrentDictionary<Type, object> _singletons =
            new ConcurrentDictionary<Type, object>();

        internal ServiceLocatorContainer()
            : this(new ServiceCreator(), new DefaultServiceLocatorSetup())
        {
        }

        internal ServiceLocatorContainer(IServiceLocatorSetup setup)
            : this(new ServiceCreator(), setup)
        {
        }

        internal ServiceLocatorContainer(
            IServiceCreator serviceCreator,
            IServiceLocatorSetup setup)
        {
            ArgumentChecker.NotNull(serviceCreator, () => serviceCreator);
            ArgumentChecker.NotNull(setup, () => setup);

            _serviceCreator = serviceCreator;

            setup.Configure(this);
        }

        public void Register<TService, TImplementation>() where TImplementation : TService
        {
            Func<TImplementation> factory = Create<TImplementation>;
            AddOrUpdate(typeof(TService), factory);
        }

        private void AddOrUpdate<TService>(Type type, Func<TService> instanceFactory)
        {
            Func<Type, Delegate[], Delegate[]> updateAction = (_, existingArray) =>
            {
                Delegate[] updatedArray = new Delegate[existingArray.Length + 1];
                Array.Copy(existingArray, updatedArray, existingArray.Length);
                updatedArray[updatedArray.Length - 1] = instanceFactory;
                return updatedArray;
            };

            _registrations.AddOrUpdate(
                type, 
                _ => new Delegate[] { instanceFactory },
                updateAction);
        }

        public void RegisterSingleton<TService, TImplementation>()
            where TImplementation : TService
        {
            Func<TImplementation> factory = GetOrCreateSingleton<TImplementation>;
            AddOrUpdate(typeof(TService), factory);
        }

        public void RegisterSingleton<TService, TImplementation>(TImplementation instance) where TImplementation : TService
        {
            Func<TImplementation> factory = () => instance;
            AddOrUpdate(typeof(TService), factory);
        }

        public TService Resolve<TService>()
        {
            Delegate[] factories;
            _registrations.TryGetValue(typeof(TService), out factories);

            var typedFactories = factories?.OfType<Func<TService>>().ToList();

            if (typedFactories == null || typedFactories.Count == 0)
            {
                throw new InvalidOperationException("No registration found for " + typeof(TService).Name);
            }

            return typedFactories[typedFactories.Count - 1]();
        }

        public bool TryResolve(Type type, out object instance)
        {
            Delegate[] factories;
            _registrations.TryGetValue(type, out factories);

            if (factories == null || factories.Length == 0)
            {
                instance = null;
                return false;
            }

            instance = factories[factories.Length - 1].DynamicInvoke();
            return true;
        }

        public IEnumerable<TService> ResolveAll<TService>()
        {
            Delegate[] factories;
            _registrations.TryGetValue(typeof(TService), out factories);

            return factories?.OfType<Func<TService>>().Select(factory => factory()) ?? Enumerable.Empty<TService>();
        }

        private T Create<T>()
        {
            return (T)_serviceCreator.Create(typeof(T), this);
        }

        private T GetOrCreateSingleton<T>()
        {
            return (T)_singletons.GetOrAdd(typeof(T), type => Create<T>());
        }
    }
}