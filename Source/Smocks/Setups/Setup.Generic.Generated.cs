using System;
using Smocks.Utility;

namespace Smocks.Setups
{
    internal partial class Setup<TReturnValue>
    {
		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1>(Func<T1, TReturnValue> generator)
		{
            if (ParameterCount != 1)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1>(Action<T1> callback)
		{
            if (ParameterCount != 1)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2>(Func<T1, T2, TReturnValue> generator)
		{
            if (ParameterCount != 2)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2>(Action<T1, T2> callback)
		{
            if (ParameterCount != 2)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3>(Func<T1, T2, T3, TReturnValue> generator)
		{
            if (ParameterCount != 3)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
		{
            if (ParameterCount != 3)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TReturnValue> generator)
		{
            if (ParameterCount != 4)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
		{
            if (ParameterCount != 4)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, TReturnValue> generator)
		{
            if (ParameterCount != 5)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback)
		{
            if (ParameterCount != 5)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, TReturnValue> generator)
		{
            if (ParameterCount != 6)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback)
		{
            if (ParameterCount != 6)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, TReturnValue> generator)
		{
            if (ParameterCount != 7)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback)
		{
            if (ParameterCount != 7)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturnValue> generator)
		{
            if (ParameterCount != 8)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
		{
            if (ParameterCount != 8)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturnValue> generator)
		{
            if (ParameterCount != 9)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
		{
            if (ParameterCount != 9)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturnValue> generator)
		{
            if (ParameterCount != 10)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
		{
            if (ParameterCount != 10)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturnValue> generator)
		{
            if (ParameterCount != 11)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> callback)
		{
            if (ParameterCount != 11)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturnValue> generator)
		{
            if (ParameterCount != 12)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> callback)
		{
            if (ParameterCount != 12)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturnValue> generator)
		{
            if (ParameterCount != 13)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> callback)
		{
            if (ParameterCount != 13)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <typeparam name="T14">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturnValue> generator)
		{
            if (ParameterCount != 14)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13)),
				(T14)Convert.ChangeType(args[13], typeof(T14))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <typeparam name="T14">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> callback)
		{
            if (ParameterCount != 14)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13)),
				(T14)Convert.ChangeType(args[13], typeof(T14))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <typeparam name="T14">The type of the argument.</typeparam>
		/// <typeparam name="T15">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturnValue> generator)
		{
            if (ParameterCount != 15)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13)),
				(T14)Convert.ChangeType(args[13], typeof(T14)),
				(T15)Convert.ChangeType(args[14], typeof(T15))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <typeparam name="T14">The type of the argument.</typeparam>
		/// <typeparam name="T15">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> callback)
		{
            if (ParameterCount != 15)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13)),
				(T14)Convert.ChangeType(args[13], typeof(T14)),
				(T15)Convert.ChangeType(args[14], typeof(T15))
				);

			return this;
		}

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <typeparam name="T14">The type of the argument.</typeparam>
		/// <typeparam name="T15">The type of the argument.</typeparam>
		/// <typeparam name="T16">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturnValue> generator)
		{
            if (ParameterCount != 16)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			ReturnValueGenerator = args => generator(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13)),
				(T14)Convert.ChangeType(args[13], typeof(T14)),
				(T15)Convert.ChangeType(args[14], typeof(T15)),
				(T16)Convert.ChangeType(args[15], typeof(T16))
				);

			return this;
		}

        /// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <typeparam name="T5">The type of the argument.</typeparam>
		/// <typeparam name="T6">The type of the argument.</typeparam>
		/// <typeparam name="T7">The type of the argument.</typeparam>
		/// <typeparam name="T8">The type of the argument.</typeparam>
		/// <typeparam name="T9">The type of the argument.</typeparam>
		/// <typeparam name="T10">The type of the argument.</typeparam>
		/// <typeparam name="T11">The type of the argument.</typeparam>
		/// <typeparam name="T12">The type of the argument.</typeparam>
		/// <typeparam name="T13">The type of the argument.</typeparam>
		/// <typeparam name="T14">The type of the argument.</typeparam>
		/// <typeparam name="T15">The type of the argument.</typeparam>
		/// <typeparam name="T16">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        public ISetup<TReturnValue> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> callback)
		{
            if (ParameterCount != 16)
            {
                throw new ArgumentException("Invalid parameter count", "callback");    
            }

			CallbackAction = args => callback(
				(T1)Convert.ChangeType(args[0], typeof(T1)),
				(T2)Convert.ChangeType(args[1], typeof(T2)),
				(T3)Convert.ChangeType(args[2], typeof(T3)),
				(T4)Convert.ChangeType(args[3], typeof(T4)),
				(T5)Convert.ChangeType(args[4], typeof(T5)),
				(T6)Convert.ChangeType(args[5], typeof(T6)),
				(T7)Convert.ChangeType(args[6], typeof(T7)),
				(T8)Convert.ChangeType(args[7], typeof(T8)),
				(T9)Convert.ChangeType(args[8], typeof(T9)),
				(T10)Convert.ChangeType(args[9], typeof(T10)),
				(T11)Convert.ChangeType(args[10], typeof(T11)),
				(T12)Convert.ChangeType(args[11], typeof(T12)),
				(T13)Convert.ChangeType(args[12], typeof(T13)),
				(T14)Convert.ChangeType(args[13], typeof(T14)),
				(T15)Convert.ChangeType(args[14], typeof(T15)),
				(T16)Convert.ChangeType(args[15], typeof(T16))
				);

			return this;
		}

	}
}