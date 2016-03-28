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
using System.Linq.Expressions;
using Mono.Cecil;
using Smocks.IL;
using Smocks.IL.Dependencies;
using Smocks.IL.Filters;
using Smocks.IL.Resolvers;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks.Injection
{
    /// <summary>
    /// Contains the default setup of the dependency injection.
    /// </summary>
    [Serializable]
    public class DefaultServiceLocatorSetup : IServiceLocatorSetup
    {
        /// <summary>
        /// Configures the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Configure(IServiceLocatorContainer container)
        {
            container.RegisterSingleton<ISetupManager, SetupManager>();
            container.RegisterSingleton<IInvocationTracker, InvocationTracker>();

            container.RegisterSingleton<Interceptor, Interceptor>();
            container.RegisterSingleton<EventInterceptor, EventInterceptor>();

            container.RegisterSingleton<ISetupMatcher, SetupMatcher>();
            container.RegisterSingleton<ITargetMatcher, TargetMatcher>();
            container.RegisterSingleton<IArgumentMatcher, ArgumentMatcher>();
            container.RegisterSingleton<IItIsMatcher, ItIsMatcher>();

            container.RegisterSingleton<IMethodDisassembler, MethodDisassembler>();
            container.RegisterSingleton<IExpressionDecompiler<Expression>, ExpressionDecompiler<Expression>>();
            container.RegisterSingleton<IExpressionDecompiler<Action>, ExpressionDecompiler<Action>>();
            container.RegisterSingleton<IExpressionCompiler, ExpressionCompiler>();
            container.RegisterSingleton<IExpressionHelper, ExpressionHelper>();
            container.RegisterSingleton<IInstructionsCompiler, DynamicMethodCompiler>();
            container.RegisterSingleton<IOpCodeMapper, OpCodeMapper>();
            container.RegisterSingleton<IMethodRewriter, MethodRewriter>();
            container.RegisterSingleton<IInstructionHelper, InstructionHelper>();
            container.RegisterSingleton<IParameterDeducer, ParameterDeducer>();
            container.RegisterSingleton<IArgumentGenerator, ArgumentGenerator>();

            container.RegisterSingleton<ITypeResolver, TypeResolver>();
            container.RegisterSingleton<IModuleResolver, ModuleResolver>();
            container.RegisterSingleton<IFieldResolver, FieldResolver>();
            container.RegisterSingleton<IMethodResolver, MethodResolver>();

            container.RegisterSingleton<SetupExtractor, SetupExtractor>();
            container.RegisterSingleton<ISetupExtractor, SetupExtractor>();
            container.RegisterSingleton<IEventTargetExtractor, EventTargetExtractor>();
            container.RegisterSingleton<IEventAccessorExtractor, EventAccessorExtractor>();

            container.RegisterSingleton<IDependencyGraphBuilder, DependencyGraphBuilder>();

            container.RegisterSingleton<IModuleFilterFactory, ModuleFilterFactory>();

            container.Register<IEqualityComparer<ModuleReference>, ModuleReferenceComparer>();

            container.Register<ISmocksContext, SmocksContext>();

            container.Register<IServiceCreator, ServiceCreator>();
        }
    }
}