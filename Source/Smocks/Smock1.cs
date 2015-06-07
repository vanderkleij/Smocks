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

namespace Smocks
{
    /// <summary>
    /// This is the entry point for users of the library. Users can use
    /// the Run method to start a Smocks session.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1508:ClosingCurlyBracketsMustNotBePrecededByBlankLine", Justification = "Generated.")]
    public partial class Smock
    {
        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static byte Run(Func<ISmocksContext, byte> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static byte Run(Configuration configuration, Func<ISmocksContext, byte> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static sbyte Run(Func<ISmocksContext, sbyte> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static sbyte Run(Configuration configuration, Func<ISmocksContext, sbyte> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static ushort Run(Func<ISmocksContext, ushort> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static ushort Run(Configuration configuration, Func<ISmocksContext, ushort> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static short Run(Func<ISmocksContext, short> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static short Run(Configuration configuration, Func<ISmocksContext, short> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static uint Run(Func<ISmocksContext, uint> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static uint Run(Configuration configuration, Func<ISmocksContext, uint> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static int Run(Func<ISmocksContext, int> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static int Run(Configuration configuration, Func<ISmocksContext, int> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static ulong Run(Func<ISmocksContext, ulong> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static ulong Run(Configuration configuration, Func<ISmocksContext, ulong> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static long Run(Func<ISmocksContext, long> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static long Run(Configuration configuration, Func<ISmocksContext, long> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static Guid Run(Func<ISmocksContext, Guid> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static Guid Run(Configuration configuration, Func<ISmocksContext, Guid> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static Uri Run(Func<ISmocksContext, Uri> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static Uri Run(Configuration configuration, Func<ISmocksContext, Uri> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static float Run(Func<ISmocksContext, float> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static float Run(Configuration configuration, Func<ISmocksContext, float> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static double Run(Func<ISmocksContext, double> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static double Run(Configuration configuration, Func<ISmocksContext, double> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static bool Run(Func<ISmocksContext, bool> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static bool Run(Configuration configuration, Func<ISmocksContext, bool> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static decimal Run(Func<ISmocksContext, decimal> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static decimal Run(Configuration configuration, Func<ISmocksContext, decimal> func)
        {
            return RunInternal(func, configuration);
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static char Run(Func<ISmocksContext, char> func)
        {
            return RunInternal(func, new Configuration());
        }

        /// <summary>
        /// Runs the specified function as a Smocks session.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result of the function.</returns>
        public static char Run(Configuration configuration, Func<ISmocksContext, char> func)
        {
            return RunInternal(func, configuration);
        }

    }
}
