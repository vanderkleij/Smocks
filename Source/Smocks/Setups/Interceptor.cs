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

using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Interceptor"/> class.
        /// </summary>
        /// <param name="setupMatcher">The setup matcher.</param>
        /// <param name="invocationTracker">The invocation tracker.</param>
        internal Interceptor(ISetupMatcher setupMatcher, IInvocationTracker invocationTracker)
        {
            ArgumentChecker.NotNull(setupMatcher, () => setupMatcher);
            ArgumentChecker.NotNull(invocationTracker, () => invocationTracker);

            _setupMatcher = setupMatcher;
            _invocationTracker = invocationTracker;
        }

        /// <summary>
        /// Intercepts the specified method.
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="arguments">The arguments provided in the original method call.</param>
        /// <param name="originalMethod">The original method.</param>
        /// <returns>The return value after interception.</returns>
        public static TReturnValue Intercept<TReturnValue>(object[] arguments, MethodBase originalMethod)
        {
            return ServiceLocator.Instance.Resolve<Interceptor>()
                .InterceptMethod<TReturnValue>(arguments, originalMethod);
        }

        /// <summary>
        /// Intercepts the specified void method.
        /// </summary>
        /// <param name="arguments">The arguments provided in the original method call.</param>
        /// <param name="originalMethod">The original method.</param>
        public static void InterceptVoid(object[] arguments, MethodBase originalMethod)
        {
            ServiceLocator.Instance.Resolve<Interceptor>()
                .InterceptVoidMethod(arguments, originalMethod);
        }

        internal TReturnValue InterceptMethod<TReturnValue>(object[] arguments, MethodBase originalMethod)
        {
            var setup = _setupMatcher.GetBestMatchingSetup(
                originalMethod, arguments) as IInternalSetup<TReturnValue>;

            _invocationTracker.Track(originalMethod, arguments, setup);

            if (setup != null)
            {
                if (setup.Exception != null)
                {
                    throw setup.Exception.Value;
                }

                if (setup.HasReturnValue)
                {
                    return setup.ReturnValue;
                }
            }

            return (TReturnValue)InvokeOriginalMethod(arguments, originalMethod);
        }

        internal void InterceptVoidMethod(object[] arguments, MethodBase originalMethod)
        {
            IInternalSetup setup = _setupMatcher.GetBestMatchingSetup(originalMethod, arguments);

            _invocationTracker.Track(originalMethod, arguments, setup);

            if (setup != null && setup.Exception != null)
            {
                throw setup.Exception.Value;
            }

            InvokeOriginalMethod(arguments, originalMethod);
        }

        private static object InvokeOriginalMethod(object[] arguments, MethodBase originalMethod)
        {
            ConstructorInfo constructor = originalMethod as ConstructorInfo;

            if (constructor != null)
            {
                return constructor.Invoke(arguments);
            }

            object target = originalMethod.IsStatic ? null : arguments[0];
            arguments = originalMethod.IsStatic ? arguments : arguments.Skip(1).ToArray();

            return originalMethod.Invoke(target, arguments);
        }
    }
}