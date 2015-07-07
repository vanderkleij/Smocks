using System;

namespace Smocks.Setups.Fluent
{
    /// <summary>
    /// Contains .Returns(...) setup methods
    /// </summary>
    /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
    public partial interface IReturns<in TReturnValue>
    {
		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        ISetup<TReturnValue> Returns<T1>(Func<T1, TReturnValue> generator);

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        ISetup<TReturnValue> Returns<T1, T2>(Func<T1, T2, TReturnValue> generator);

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        ISetup<TReturnValue> Returns<T1, T2, T3>(Func<T1, T2, T3, TReturnValue> generator);

		/// <summary>
        /// Configures a callback that returns the return value.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <param name="generator">The generator.</param>
        /// <returns>The setup.</returns>
        ISetup<TReturnValue> Returns<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TReturnValue> generator);

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
        ISetup<TReturnValue> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TReturnValue> generator);

	    }
}