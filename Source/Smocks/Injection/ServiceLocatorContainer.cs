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
using Smocks.Utility;

namespace Smocks.Injection
{
    internal class ServiceLocatorContainer : IServiceLocatorContainer
    {
        private readonly ConcurrentDictionary<Type, Delegate> _registrations =
            new ConcurrentDictionary<Type, Delegate>();

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
            _registrations.AddOrUpdate(typeof(TService), factory, (type, existing) => factory);
        }

        public void RegisterSingleton<TService, TImplementation>()
            where TImplementation : TService
        {
            Func<TImplementation> factory = GetOrCreateSingleton<TImplementation>;
            _registrations.AddOrUpdate(typeof(TService), factory, (type, existing) => factory);
        }

        public void RegisterSingleton<TService, TImplementation>(TImplementation instance) where TImplementation : TService
        {
            Func<TImplementation> factory = () => instance;
            _registrations.AddOrUpdate(typeof(TService), factory, (type, existing) => factory);
        }

        public T Resolve<T>()
        {
            Delegate factory;
            _registrations.TryGetValue(typeof(T), out factory);

            Func<T> typedFactory = factory as Func<T>;

            if (typedFactory == null)
            {
                throw new InvalidOperationException("No registration found for " + typeof(T).Name);
            }

            return typedFactory();
        }

        public bool TryResolve(Type type, out object instance)
        {
            Delegate factory;
            _registrations.TryGetValue(type, out factory);

            if (factory == null)
            {
                instance = null;
                return false;
            }

            instance = factory.DynamicInvoke();
            return true;
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