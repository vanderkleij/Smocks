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
using System.Runtime.Serialization;
using Smocks.Setups;
using Smocks.Utility;
using System.Text;

namespace Smocks.Exceptions
{
    /// <summary>
    /// An exception caused by the fact that a <see cref="ISetup"/> expected
    /// a certain number of calls to its assigned method, but this expectation
    /// was not met upon verifying.
    /// </summary>
    [Serializable]
    public class VerificationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationException" /> class.
        /// </summary>
        /// <param name="notMatchedSetups">The not matched setups.</param>
        internal VerificationException(IEnumerable<IInternalSetupBase> notMatchedSetups)
            : base(GetMessage(notMatchedSetups))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected VerificationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private static string GetMessage(IEnumerable<IInternalSetupBase> notMatchedSetups)
        {
            ArgumentChecker.NotNull(notMatchedSetups, () => notMatchedSetups);

            StringBuilder message = new StringBuilder();

            foreach (IInternalSetupBase setup in notMatchedSetups)
            {
                if (!string.IsNullOrEmpty(message.ToString()))
                {
                    message.Append(",");
                }

                message.Append(Environment.NewLine);
                message.Append(setup.MethodCall.Method.ReflectedType);
                message.Append(".");
                message.Append(setup.MethodCall.Method.Name);
                message.Append("(");

                if (setup.MethodCall.Arguments.Count > 0)
                {
                    message.Append(string.Join(",", setup.MethodCall.Arguments));
                }

                message.Append(")");
            }

            return "Verification failed for method setups : " + message.ToString();
        }
    }
}