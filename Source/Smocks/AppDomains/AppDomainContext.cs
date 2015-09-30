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
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Smocks.IL;
using Smocks.Logging;
using Smocks.Serialization;
using Smocks.Utility;

namespace Smocks.AppDomains
{
    internal class AppDomainContext : IAppDomainContext
    {
        private readonly AppDomain _appDomain;
        private readonly IAssemblyLoaderFactory _assemblyLoaderFactory;
        private readonly ConcurrentBag<string> _filesToDelete = new ConcurrentBag<string>();
        private readonly ReadOnlyCollection<ISerializableLambdaFilter> _lambdaFilters;
        private readonly Logger _logger;
        private readonly ISerializer _serializer;

        internal AppDomainContext(IAssemblyRewriter assemblyRewriter)
            : this(assemblyRewriter, null)
        {
        }

        internal AppDomainContext(IAssemblyRewriter assemblyRewriter, Logger logger)
            : this(
                new AssemblyLoaderFactory(AppDomain.CurrentDomain.BaseDirectory, assemblyRewriter),
                new Serializer(),
                logger)
        {
        }

        internal AppDomainContext(
            IAssemblyLoaderFactory assemblyLoaderFactory,
            ISerializer serializer,
            Logger logger,
            params ISerializableLambdaFilter[] lambdaFilters)
        {
            _assemblyLoaderFactory = assemblyLoaderFactory;
            _serializer = serializer;
            _lambdaFilters = lambdaFilters.ToList().AsReadOnly();
            _logger = logger;

            var domainSetup = new AppDomainSetup
            {
                ApplicationBase = null,
                PrivateBinPathProbe = "true",
                LoaderOptimization = LoaderOptimization.SingleDomain
            };

            _appDomain = AppDomain.CreateDomain("Smocks-" + Guid.NewGuid(), null, domainSetup);

            object[] constructorArguments = { };
            var resolver = (AssemblyResolver)_appDomain.CreateInstanceFromAndUnwrap(
                typeof(AssemblyResolver).Assembly.Location,
                typeof(AssemblyResolver).FullName,
                true,
                BindingFlags.Default,
                null,
                constructorArguments,
                null,
                null);

            resolver.AssemblyLoaderFactory = assemblyLoaderFactory;
        }

        public AppDomain AppDomain
        {
            get { return _appDomain; }
        }

        public void Dispose()
        {
            AppDomain.Unload(_appDomain);

            _assemblyLoaderFactory.Dispose();

            foreach (string fileToDelete in _filesToDelete.Distinct())
            {
                DeleteFile(fileToDelete);
            }
        }

        /// <summary>
        /// Gets the assemblies that have been loaded into the execution context of this application domain.
        /// </summary>
        /// <returns>
        /// A collection of assemblies in this application domain.
        /// </returns>
        public IEnumerable<Assembly> GetAssemblies()
        {
            return _appDomain.GetAssemblies();
        }

        public void Invoke(Action action)
        {
            SerializableLambda serializableAction = SerializableLambda.Create(action, _serializer);

            // Invoke the action in the other AppDomain and get the return value +
            // serialized version of the target in the other app domain.
            InvocationResult result = InvokeLambda(serializableAction);

            // Deserialize the other app domain's target and copy it to our local target.
            _serializer.Populate(result.SerializedTarget, action.Target);
        }

        public void Invoke<T>(Action<T> action, T parameter)
        {
            SerializableLambda<Unit> serializableFunc = SerializableLambda.Create(action, _serializer);
            InvokeSerializableLambda(serializableFunc, action.Target, parameter);
        }

        public T Invoke<T>(Func<T> func)
        {
            SerializableLambda<T> serializableFunc = SerializableLambda.Create(func, _serializer);
            return InvokeSerializableLambda(serializableFunc, func.Target);
        }

        public TResult Invoke<T1, TResult>(Func<T1, TResult> func, T1 parameter)
        {
            SerializableLambda<TResult> serializableFunc = SerializableLambda.Create(func, _serializer);
            return InvokeSerializableLambda(serializableFunc, func.Target, parameter);
        }

        /// <summary>
        /// Registers a file for deletion
        /// </summary>
        /// <param name="location">The location.</param>
        public void RegisterForDeletion(string location)
        {
            _filesToDelete.Add(location);
        }

        public void SetData<T>(string key, T value)
        {
            _appDomain.SetData(key, value);
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return _appDomain.FriendlyName;
        }

        private T Create<T>() where T : MarshalByRefObject
        {
            return (T)_appDomain.CreateInstanceAndUnwrap(
                typeof(T).Assembly.FullName,
                typeof(T).FullName,
                null);
        }

        private void DeleteFile(string fileToDelete)
        {
            try
            {
                File.Delete(fileToDelete);
            }
            catch (Exception exception)
            {
                _logger.Error("Could not delete file {0}: {1}", fileToDelete, exception);
            }
        }

        private SerializableLambda<T> FilterLambda<T>(SerializableLambda<T> serializableLambda)
        {
            return _lambdaFilters.Aggregate(serializableLambda,
                (current, filter) => filter.Filter(this, current));
        }

        private InvocationResult<T> InvokeLambda<T>(
            SerializableLambda<T> serializableLambda,
            params object[] arguments)
        {
            SerializableLambdaInvoker invoker = Create<SerializableLambdaInvoker>();

            InvocationResult<T> result;

            try
            {
                SerializableLambda<T> filtered = FilterLambda(serializableLambda);
                result = invoker.Invoke(filtered, _serializer, arguments);
            }
            catch (FileLoadException exception)
            {
                _logger.Error("Error invoking SerializableLambda: {0}", exception.GetBaseException());
                throw exception.GetBaseException();
            }

            return result;
        }

        private T InvokeSerializableLambda<T>(SerializableLambda<T> serializableFunc, object target,
            params object[] arguments)
        {
            InvocationResult<T> result;

            try
            {
                // Invoke the action in the other AppDomain and get the return value +
                // serialized version of the target in the other app domain.
                result = InvokeLambda(serializableFunc, arguments);
            }
            catch (TargetInvocationException exception)
            {
                _logger.Error("Error invoking SerializableLambda: {0}", exception.GetBaseException());

                // Unwrap the exception
                throw exception.GetBaseException();
            }

            // Deserialize the other app domain's target and copy it to our local target.
            _serializer.Populate(result.SerializedTarget, target);

            return result.ReturnValue;
        }
    }
}