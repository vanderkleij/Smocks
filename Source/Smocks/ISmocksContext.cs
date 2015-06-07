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
using System.Linq.Expressions;
using Smocks.Setups;

namespace Smocks
{
    /// <summary>
    /// The context for a Smocks session. The user can use the context to configure
    /// setups and verify expectations.
    /// </summary>
    public interface ISmocksContext
    {
        /// <summary>
        /// Creates a setup for the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A setup.</returns>
        ISetup Setup(Expression<Action> expression);

        /// <summary>
        /// Creates a setup for the specified expression.
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>A setup.</returns>
        ISetup<TReturnValue> Setup<TReturnValue>(Expression<Func<TReturnValue>> expression);

        /// <summary>
        /// Verifies the expectations configured for the setups.
        /// </summary>
        void Verify();
    }
}