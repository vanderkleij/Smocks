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
#endregion License

using System.Diagnostics.CodeAnalysis;

namespace Smocks.Tests.TestUtility
{
    [ExcludeFromCodeCoverage]
    internal class TestFunctions
    {
        public static bool StringOutParameter(string arg1, out string arg2)
        {
            arg2 = arg1;
            return false;
        }

        internal static T EightArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8)
        {
            return arg1;
        }

        internal static T ElevenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11)
        {
            return arg1;
        }

        internal static T FifteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13, T arg14, T arg15)
        {
            return arg1;
        }

        internal static T FiveArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5)
        {
            return arg1;
        }

        internal static T FourArguments<T>(T arg1, T arg2, T arg3, T arg4)
        {
            return arg1;
        }

        internal static T FourteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13, T arg14)
        {
            return arg1;
        }

        internal static bool IntRefParameter(string arg1, ref int arg2)
        {
            arg2 += 10;
            return false;
        }

        internal static bool LongRefParameter(string arg1, ref long arg2)
        {
            return false;
        }

        internal static T NineArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9)
        {
            return arg1;
        }

        internal static T OneArgument<T>(T arg1)
        {
            return arg1;
        }

        internal static T SevenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7)
        {
            return arg1;
        }

        internal static T SixArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6)
        {
            return arg1;
        }

        internal static T SixteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13, T arg14, T arg15, T arg16)
        {
            return arg1;
        }

        internal static bool StringRefParameter(string arg1, ref string arg2)
        {
            return false;
        }

        internal static T TenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10)
        {
            return arg1;
        }

        internal static T ThirteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13)
        {
            return arg1;
        }

        internal static T ThreeArguments<T>(T arg1, T arg2, T arg3)
        {
            return arg1;
        }

        internal static T TwelveArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12)
        {
            return arg1;
        }

        internal static T TwoArguments<T>(T arg1, T arg2)
        {
            return arg1;
        }

        internal static void VoidEightArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8)
        {
        }

        internal static void VoidElevenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11)
        {
        }

        internal static void VoidFifteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13, T arg14, T arg15)
        {
        }

        internal static void VoidFiveArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5)
        {
        }

        internal static void VoidFourArguments<T>(T arg1, T arg2, T arg3, T arg4)
        {
        }

        internal static void VoidFourteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13, T arg14)
        {
        }

        internal static void VoidNineArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9)
        {
        }

        internal static void VoidOneArgument<T>(T arg1)
        {
        }

        internal static void VoidSevenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7)
        {
        }

        internal static void VoidSixArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6)
        {
        }

        internal static void VoidSixteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13, T arg14, T arg15, T arg16)
        {
        }

        internal static void VoidTenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10)
        {
        }

        internal static void VoidThirteenArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12, T arg13)
        {
        }

        internal static void VoidThreeArguments<T>(T arg1, T arg2, T arg3)
        {
        }

        internal static void VoidTwelveArguments<T>(T arg1, T arg2, T arg3, T arg4, T arg5, T arg6,
            T arg7, T arg8, T arg9, T arg10, T arg11, T arg12)
        {
        }

        internal static void VoidTwoArguments<T>(T arg1, T arg2)
        {
        }

        internal static void VoidZeroArguments<T>()
        {
        }

        internal static T ZeroArguments<T>()
        {
            return default(T);
        }
    }
}