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
using System.Reflection;
using Smocks.Serialization;
using Smocks.Utility;

namespace Smocks.AppDomains
{
    /// <summary>
    /// Represents a lambda (typicaly, a
    /// <see cref="Func{TResult}" /> that can be serialized.
    /// This is particularly useful to transmit a lambda to a different
    /// <see cref="AppDomain" />.
    /// </summary>
    [Serializable]
    public class SerializableLambda : SerializableLambda<Unit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableLambda"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetValues">The values of the public properties/fields of the target.</param>
        private SerializableLambda(MethodInfo method, Type targetType, Dictionary<string, object> targetValues)
            : base(method, targetType, targetValues)
        {
        }

        /// <summary>
        /// Creates a <see cref="SerializableLambda{T}" /> for a given <see cref="Func{TResult}" />.
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="func">The lambda.</param>
        /// <param name="serializer">The serializer used to serialize the target of the lambda.</param>
        /// <returns>
        /// A <see cref="SerializableLambda{T}" /> for the provided lambda.
        /// </returns>
        internal static SerializableLambda<TReturnValue> Create<TReturnValue>(
            Func<TReturnValue> func, ISerializer serializer)
        {
            return SerializableLambda<TReturnValue>.Create(func, serializer);
        }

        /// <summary>
        /// Creates a <see cref="SerializableLambda{T}"/> for a given <see cref="Action"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="serializer">The serializer used to serialize the target of the lambda.</param>
        /// <returns>A <see cref="SerializableLambda"/> for the provided lambda.</returns>
        internal static SerializableLambda Create(Action action, ISerializer serializer)
        {
            MethodInfo method = action.Method;
            Type targetType = action.Target != null ? action.Target.GetType() : null;
            var serializedTarget = action.Target != null ? serializer.Serialize(action.Target) : null;

            return new SerializableLambda(method, targetType, serializedTarget);
        }

        /// <summary>
        /// Creates a <see cref="SerializableLambda{T}"/> for a given <see cref="Action"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A <see cref="SerializableLambda"/> for the provided lambda.</returns>
        internal static SerializableLambda Create(Action action)
        {
            return Create(action, new Serializer());
        }

        /// <summary>
        /// Creates a <see cref="SerializableLambda{TReturnValue}" /> for a given
        /// <see cref="Func{T1, TReturnValue}" />.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="func">The lambda.</param>
        /// <returns>
        /// A <see cref="SerializableLambda{TReturnValue}" /> for the provided lambda.
        /// </returns>
        internal static SerializableLambda<TReturnValue> Create<T1, TReturnValue>(
            Func<T1, TReturnValue> func)
        {
            return Create(func, new Serializer());
        }

        /// <summary>
        /// Creates a <see cref="SerializableLambda{TReturnValue}" /> for a given
        /// <see cref="Func{T1, TReturnValue}" />.
        /// </summary>
        /// <typeparam name="T1">The type of the the first argument.</typeparam>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="func">The lambda.</param>
        /// <param name="serializer">The serializer used to serialize the target of the lambda.</param>
        /// <returns>
        /// A <see cref="SerializableLambda{TReturnValue}" /> for the provided lambda.
        /// </returns>
        internal static SerializableLambda<TReturnValue> Create<T1, TReturnValue>(
            Func<T1, TReturnValue> func, ISerializer serializer)
        {
            return SerializableLambda<TReturnValue>.Create(func, serializer);
        }
    }

    /// <summary>
    /// Represents a lambda (typicaly, a
    /// <see cref="Func{TResult}" /> that can be serialized.
    /// This is particularly useful to transmit a lambda to a different
    /// <see cref="AppDomain" />.
    /// </summary>
    /// <typeparam name="TReturnValue">The return value of the lambda.</typeparam>
    [Serializable]
    public class SerializableLambda<TReturnValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableLambda{TReturnValue}"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetValues">The values of the public properties/fields of the target.</param>
        internal SerializableLambda(MethodInfo method, Type targetType, Dictionary<string, object> targetValues)
        {
            Method = method;
            TargetType = targetType;
            TargetValues = targetValues;
        }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public MethodInfo Method { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the target of the lambda.
        /// </summary>
        public Type TargetType { get; protected set; }

        /// <summary>
        /// Gets the values of the public properties/fields of the target.
        /// </summary>
        public Dictionary<string, object> TargetValues { get; private set; }

        /// <summary>
        /// Creates a <see cref="SerializableLambda{T}"/> for a given <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="func">The lambda.</param>
        /// <param name="serializer">The serializer used to serialize the target of the lambda.</param>
        /// <returns>A <see cref="SerializableLambda{T}"/> for the provided lambda.</returns>
        internal static SerializableLambda<TReturnValue> Create(Delegate func, ISerializer serializer)
        {
            MethodInfo method = func.Method;
            Type targetType = func.Target != null ? func.Target.GetType() : null;
            Dictionary<string, object> targetValues = serializer.Serialize(func.Target);

            return new SerializableLambda<TReturnValue>(method, targetType, targetValues);
        }
    }
}