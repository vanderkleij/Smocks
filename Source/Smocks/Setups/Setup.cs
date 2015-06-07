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
using Smocks.Utility;

namespace Smocks.Setups
{
    internal class Setup : IInternalSetup
    {
        public Setup(MethodCallInfo methodCall)
        {
            ArgumentChecker.NotNull(methodCall, () => methodCall);

            MethodCall = methodCall;
        }

        public Lazy<Exception> Exception { get; private set; }

        public MethodCallInfo MethodCall { get; private set; }

        public bool Verify { get; private set; }

        public void Throws<TException>() where TException : Exception, new()
        {
            Exception = new Lazy<Exception>(() => new TException());
        }

        public void Throws<TException>(TException exception) where TException : Exception
        {
            Exception = new Lazy<Exception>(() => exception);
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return MethodCall.ToString();
        }

        public void Verifiable()
        {
            Verify = true;
        }
    }
}