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
#endregion License

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Smocks.Injection;
using Smocks.Utility;

namespace Smocks.Setups
{
    /// <summary>
    /// This class is used by the internals of Smocks in the interception of
    /// method calls that are the target of a setup.
    /// </summary>
    public class Interceptor
    {
        private readonly IInvocationTracker _invocationTracker;
        private readonly ISetupMatcher _setupMatcher;
        private readonly IExpressionHelper _expressionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interceptor" /> class.
        /// </summary>
        /// <param name="setupMatcher">The setup matcher.</param>
        /// <param name="invocationTracker">The invocation tracker.</param>
        /// <param name="expressionHelper">The expression helper.</param>
        internal Interceptor(ISetupMatcher setupMatcher, IInvocationTracker invocationTracker,
            IExpressionHelper expressionHelper)
        {
            ArgumentChecker.NotNull(setupMatcher, () => setupMatcher);
            ArgumentChecker.NotNull(invocationTracker, () => invocationTracker);
            ArgumentChecker.NotNull(expressionHelper, () => expressionHelper);

            _setupMatcher = setupMatcher;
            _invocationTracker = invocationTracker;
            _expressionHelper = expressionHelper;
        }

        /// <summary>
        /// Intercepts the specified method.
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="arguments">The arguments provided in the original method call.</param>
        /// <param name="originalMethod">The original method.</param>
        /// <returns>A result that specifies whether the method was intercepted
        /// and what the return value is, if it was.</returns>
        public static InterceptorResult<TReturnValue> Intercept<TReturnValue>(object[] arguments, MethodBase originalMethod)
        {
            return ServiceLocator.Instance.Resolve<Interceptor>()
                .InterceptMethod<TReturnValue>(arguments, originalMethod);
        }

        /// <summary>
        /// Intercepts the specified void method.
        /// </summary>
        /// <param name="arguments">The arguments provided in the original method call.</param>
        /// <param name="originalMethod">The original method.</param>
        /// <returns>A result that specifies whether the method was intercepted.</returns>
        public static InterceptorResult InterceptVoid(object[] arguments, MethodBase originalMethod)
        {
            return ServiceLocator.Instance.Resolve<Interceptor>()
                .InterceptVoidMethod(arguments, originalMethod);
        }

        internal InterceptorResult<TReturnValue> InterceptMethod<TReturnValue>(object[] arguments, MethodBase originalMethod)
        {
            var setup = _setupMatcher.GetBestMatchingSetup(
                originalMethod, arguments) as IInternalSetup<TReturnValue>;

            _invocationTracker.Track(originalMethod, arguments, setup);

            if (setup != null)
            {
                HandleSetup(arguments, setup, originalMethod);

                TReturnValue returnValue = setup.HasReturnValue
                    ? setup.GetReturnValue(arguments)
                    : default(TReturnValue);

                return new InterceptorResult<TReturnValue>(true, returnValue);
            }

            return new InterceptorResult<TReturnValue>(false, default(TReturnValue));
        }

        internal InterceptorResult InterceptVoidMethod(object[] arguments, MethodBase originalMethod)
        {
            IInternalSetup setup = _setupMatcher.GetBestMatchingSetup(originalMethod, arguments);

            _invocationTracker.Track(originalMethod, arguments, setup);

            if (setup == null)
            {
                return new InterceptorResult(false);
            }

            HandleSetup(arguments, setup, originalMethod);

            return new InterceptorResult(true);
        }

        private void HandleSetup(object[] arguments, IInternalSetupBase setup, MethodBase originalMethod)
        {
            if (setup.Exception != null)
            {
                throw setup.Exception.Value;
            }

            if (setup.CallbackAction != null)
            {
                setup.CallbackAction(arguments);
            }

            HandleOutParameters(arguments, setup, originalMethod);
        }

        private void HandleOutParameters(object[] arguments, IInternalSetupBase setup, MethodBase originalMethod)
        {
            ParameterInfo[] parameters = originalMethod.GetParameters();

            for (int i = 0; i < parameters.Length; ++i)
            {
                int offset = originalMethod.IsStatic ? 0 : 1;

                if (parameters[i].IsOut)
                {
                    arguments[i + offset] = _expressionHelper.GetValue(setup.MethodCall.Arguments[i + offset]);
                }
            }
        }
    }
}