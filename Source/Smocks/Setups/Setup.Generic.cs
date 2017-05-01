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
using Smocks.Utility;

namespace Smocks.Setups
{
    internal partial class Setup<TReturnValue> : SetupBase, IInternalSetup<TReturnValue>
    {
        private TReturnValue _constantReturnValue;

        private Func<object[], TReturnValue> _returnValueGenerator;

        public Setup(MethodCallInfo methodCall)
            : base(methodCall)
        {
        }

        public TReturnValue ConstantReturnValue
        {
            get
            {
                return _constantReturnValue;
            }

            set
            {
                _constantReturnValue = value;
                ReturnValueGenerator = null;
                HasConstantReturnValue = true;
            }
        }

        public bool HasConstantReturnValue { get; private set; }

        public bool HasReturnValue => HasConstantReturnValue || ReturnValueGenerator != null;

        public Func<object[], TReturnValue> ReturnValueGenerator
        {
            get
            {
                return _returnValueGenerator;
            }

            set
            {
                _returnValueGenerator = value;
                HasConstantReturnValue = false;
            }
        }

        public TReturnValue GetReturnValue(object[] arguments)
        {
            if (HasConstantReturnValue)
            {
                return ConstantReturnValue;
            }

            if (ReturnValueGenerator != null)
            {
                return ReturnValueGenerator(arguments.Skip(ArgumentsToSkipInCallbacks).ToArray());
            }

            throw new InvalidOperationException("No return value specified");
        }

        public ISetup<TReturnValue> Returns(TReturnValue returnValue)
        {
            ConstantReturnValue = returnValue;
            HasConstantReturnValue = true;

            return this;
        }

        public ISetup<TReturnValue> Returns(Func<TReturnValue> generator)
        {
            ReturnValueGenerator = args => generator();
            return this;
        }

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback(Action callback)
        {
            if (ParameterCount != 0)
            {
                throw new ArgumentException("Invalid parameter count", nameof(callback));
            }

            CallbackAction = args => callback();
            return this;
        }
    }
}