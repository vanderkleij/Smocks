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
using System.Runtime.Serialization;
using Smocks.AppDomains;
using Smocks.IL;
using Smocks.IL.Dependencies;
using Smocks.IL.Filters;
using Smocks.Injection;
using Smocks.Logging;
using Smocks.Utility;

namespace Smocks
{
    /// <summary>
    /// This is the entry point for users of the library. Users can use
    /// the Run method to start a Smocks session.
    /// </summary>
    public partial class Smock
    {
        private readonly IDependencyGraphBuilder _dependencyGraphBuilder;
        private readonly IModuleFilterFactory _moduleFilterFactory;
        private readonly IServiceLocator _serviceLocator;
        private readonly ISetupExtractor _setupExtractor;
        private readonly IEventTargetExtractor _eventTargetExtractor;

        private Smock(IServiceLocator serviceLocator)
        {
            ArgumentChecker.NotNull(serviceLocator, () => serviceLocator);

            _setupExtractor = serviceLocator.Resolve<ISetupExtractor>();
            _eventTargetExtractor = serviceLocator.Resolve<IEventTargetExtractor>();

            _dependencyGraphBuilder = serviceLocator.Resolve<IDependencyGraphBuilder>();
            _moduleFilterFactory = serviceLocator.Resolve<IModuleFilterFactory>();

            _serviceLocator = serviceLocator;
        }

        /// <summary>
        /// Runs the specified action as a Smocks session.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void Run(Action<ISmocksContext> action)
        {
            Run(new Configuration(), action);
        }

        /// <summary>
        /// Runs the specified action as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="action">The action.</param>
        public static void Run(Configuration configuration, Action<ISmocksContext> action)
        {
            Smock context = new Smock(CreateServiceLocator(configuration));
            context.RunAction(action, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <typeparam name="TSerializable">A serializable type.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function. The result is serialized across
        /// AppDomains and thereforr must be serializable.</returns>
        public static TSerializable Run<TSerializable>(Func<ISmocksContext, TSerializable> func)
            where TSerializable : ISerializable
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <typeparam name="TSerializable">A serializable type.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        /// The result of the function. The result is serialized across
        /// AppDomains and thereforr must be serializable.
        /// </returns>
        public static TSerializable Run<TSerializable>(
            Configuration configuration,
            Func<ISmocksContext, TSerializable> func) where TSerializable : ISerializable
        {
            return RunInternal(func, configuration);
        }

        private static IServiceLocator CreateServiceLocator(Configuration configuration)
        {
            return new ServiceLocator(new ServiceLocatorContainer(configuration.ServiceLocatorSetup));
        }

        private static T RunInternal<T>(Func<ISmocksContext, T> func, Configuration configuration)
        {
            Smock context = new Smock(CreateServiceLocator(configuration));
            return context.RunFunc(func, configuration);
        }

        private IAssemblyRewriter CreateAssemblyRewriter(Delegate @delegate, Configuration configuration)
        {
            var dependencyGraph = _dependencyGraphBuilder.BuildGraphForMethod(@delegate.Method);
            var setups = _setupExtractor.GetSetups(@delegate.Method, @delegate.Target).ToList();
            var eventSetups = _eventTargetExtractor.GetTargets(@delegate.Method, @delegate.Target).ToList();

            var moduleFilter = _moduleFilterFactory.GetFilter(configuration.Scope, dependencyGraph);
            var rewriter = new AssemblyRewriter(configuration, setups.Concat(eventSetups),
                _serviceLocator.Resolve<IMethodRewriter>(), moduleFilter, _serviceLocator.ResolveAll<IAssemblyPostProcessor>());
            return rewriter;
        }

        private void RunAction(Action<ISmocksContext> action, Configuration configuration)
        {
            IAssemblyRewriter rewriter = CreateAssemblyRewriter(action, configuration);

            using (AppDomainContext context = new AppDomainContext(rewriter, configuration.Logger))
            {
                configuration.Logger.Info("Creating service locator for app domain {0}", context);
                context.Invoke(new Action(() => ServiceLocator.Instance = CreateServiceLocator(configuration)));

                configuration.Logger.Info("Invoking action in app domain {0}", context);
                context.Invoke(action, _serviceLocator.Resolve<ISmocksContext>());
            }
        }

        private T RunFunc<T>(Func<ISmocksContext, T> func, Configuration configuration)
        {
            IAssemblyRewriter rewriter = CreateAssemblyRewriter(func, configuration);

            using (AppDomainContext context = new AppDomainContext(rewriter, configuration.Logger))
            {
                context.Invoke(new Action(() => ServiceLocator.Instance = CreateServiceLocator(configuration)));
                return context.Invoke(func, _serviceLocator.Resolve<ISmocksContext>());
            }
        }
    }
}