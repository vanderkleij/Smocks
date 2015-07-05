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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Smocks.Tests.TestUtility
{
    [ExcludeFromCodeCoverage]
    internal static class AppDomainUtility
    {
        internal static void InvokeInOtherAppDomain<T1, T2>(T1 arg1, T2 arg2, Action<T1, T2> method)
        {
            InTemporaryAppDomain(appDomain =>
            {
                var invoker = new Invoker<T1, T2>(arg1, arg2, method);

                try
                {
                    appDomain.DoCallBack(invoker.Invoke);
                }
                catch (TargetInvocationException exception)
                {
                    throw exception.GetBaseException();
                }
            });
        }

        internal static void InvokeInOtherAppDomain<T>(T argument, Action<T> method)
        {
            InTemporaryAppDomain(appDomain =>
            {
                var invoker = new Invoker<T>(argument, method);

                try
                {
                    appDomain.DoCallBack(invoker.Invoke);
                }
                catch (TargetInvocationException exception)
                {
                    throw exception.GetBaseException();
                }
            });
        }

        private static void InTemporaryAppDomain(Action<AppDomain> action)
        {
            var setup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory };
            var appDomain = AppDomain.CreateDomain("TempDomain", null, setup);

            action(appDomain);

            AppDomain.Unload(appDomain);
        }

        [Serializable]
        private class Invoker<T>
        {
            private readonly Action<T> _action;
            private readonly T _value;

            public Invoker(T value, Action<T> action)
            {
                _action = action;
                _value = value;
            }

            public void Invoke()
            {
                _action(_value);
            }
        }

        [Serializable]
        private class Invoker<T1, T2>
        {
            private readonly Action<T1, T2> _action;
            private readonly T1 _arg1;
            private readonly T2 _arg2;

            public Invoker(T1 arg1, T2 arg2, Action<T1, T2> action)
            {
                _action = action;
                _arg1 = arg1;
                _arg2 = arg2;
            }

            public void Invoke()
            {
                _action(_arg1, _arg2);
            }
        }
    }
}