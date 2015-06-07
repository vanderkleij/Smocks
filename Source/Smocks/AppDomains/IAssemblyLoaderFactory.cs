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
using System.Reflection;

namespace Smocks.AppDomains
{
    /// <summary>
    /// Provides a mechanism for loading an assembly, given its <see cref="AssemblyName"/>.
    /// </summary>
    public interface IAssemblyLoaderFactory : IDisposable
    {
        /// <summary>
        /// Gets a loader for an assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <returns>A loader for the specified assembly, or null if none could be created.</returns>
        IAssemblyLoader GetLoaderForAssembly(AssemblyName assemblyName);

        /// <summary>
        /// Registers an assembly by its full name and location. If you then get
        /// a loader for the specified assembly name, a loader based on the provided
        /// location will be returned.
        /// </summary>
        /// <param name="fullName">The full name of the assembly.</param>
        /// <param name="assemblyLocation">The assembly location.</param>
        void RegisterAssembly(string fullName, string assemblyLocation);
    }
}