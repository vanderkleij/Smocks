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

namespace Smocks.Utility
{
    /// <summary>
    /// Used to validate method arguments.
    /// </summary>
    internal class ArgumentChecker
    {
        private static readonly ExpressionHelper ExpressionHelper = new ExpressionHelper();

        /// <summary>
        /// Asserts the specified condition.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to throw if the condition is false.</typeparam>
        /// <param name="condition">The condition to assert.</param>
        /// <param name="exceptionArguments">The exception constructor arguments.</param>
        public static void Assert<TException>(bool condition, params object[] exceptionArguments)
            where TException : ArgumentException
        {
            if (!condition)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), exceptionArguments);
            }
        }

        /// <summary>
        /// Checks that the provided argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="selector">The selector.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if argument is null.</exception>
        public static void NotNull<T>(T argument, Expression<Func<T>> selector)
            where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(ExpressionHelper.GetField(selector).Name);
            }
        }

        /// <summary>
        /// Checks that the provided argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if argument is null.</exception>
        public static void NotNull<T>(T argument, string parameterName)
            where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}