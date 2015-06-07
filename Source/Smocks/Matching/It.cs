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

namespace Smocks.Matching
{
    /// <summary>
    /// Provides functionality similar to Moq's It class. Both implementations
    /// are interchangeable for use in Smocks. Smock provides one for users
    /// that don't use Moq.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class It
    {
        /// <summary>
        /// Matches any instance of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to match.</typeparam>
        /// <returns>The default value of <see cref="T"/>.</returns>
        public static T IsAny<T>()
        {
            return default(T);
        }
    }
}