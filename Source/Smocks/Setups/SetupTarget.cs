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

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Smocks.Utility;

namespace Smocks.Setups
{
    /// <summary>
    /// Represents that target of a <see cref="ISetup"/>.
    /// </summary>
    internal class SetupTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupTarget" /> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="method">The method.</param>
        internal SetupTarget(Expression expression, MethodBase method)
        {
            ArgumentChecker.NotNull(expression, () => expression);
            ArgumentChecker.NotNull(method, () => method);

            Expression = expression;
            Method = method;
        }

        /// <summary>
        /// Gets the expression that selects the target of the setup.
        /// </summary>
        public Expression Expression { get; private set; }

        /// <summary>
        /// Gets the method that is the target of the setup.
        /// </summary>
        public MethodBase Method { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}