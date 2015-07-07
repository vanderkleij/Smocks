using System;

namespace Smocks.Setups.Fluent
{
    /// <summary>
    /// Contains .Callback(...) setup methods
    /// </summary>
    public partial interface ICallback
    {
		/// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        ISetup Callback<T1>(Action<T1> callback);

		/// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        ISetup Callback<T1, T2>(Action<T1, T2> callback);

		/// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        ISetup Callback<T1, T2, T3>(Action<T1, T2, T3> callback);

		/// <summary>
        /// Configures a callback that is invoked when the setup's target is invoked.
        /// </summary>
		/// <typeparam name="T1">The type of the argument.</typeparam>
		/// <typeparam name="T2">The type of the argument.</typeparam>
		/// <typeparam name="T3">The type of the argument.</typeparam>
		/// <typeparam name="T4">The type of the argument.</typeparam>
		/// <param name="callback">The callback.</param>
        /// <returns>The setup.</returns>
        ISetup Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> callback);

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
        ISetup Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> callback);

	    }
}