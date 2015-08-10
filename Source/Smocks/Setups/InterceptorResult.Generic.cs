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

namespace Smocks.Setups
{
    /// <summary>
    /// The result of a (possible) method interception.
    /// </summary>
    /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
    public class InterceptorResult<TReturnValue> : InterceptorResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorResult{TReturnValue}"/> class.
        /// </summary>
        /// <param name="intercepted">if set to <c>true</c>, the method call was intercepted.</param>
        /// <param name="returnValue">The replacement return value.</param>
        public InterceptorResult(bool intercepted, TReturnValue returnValue)
            : base(intercepted)
        {
            ReturnValue = returnValue;
        }

        /// <summary>
        /// Gets or sets the replacement return value.
        /// </summary>
        public TReturnValue ReturnValue { get; set; }
    }
}