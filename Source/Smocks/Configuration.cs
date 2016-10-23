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

using System;
using Smocks.Injection;
using Smocks.Logging;
using Smocks.Utility;

namespace Smocks
{
    /// <summary>
    /// Configuration options for the <see cref="Smock"/>.
    /// </summary>
    public class Configuration : MarshalByRefObject
    {
        private Scope _scope = Scope.DirectReferences;
        private IServiceLocatorSetup _serviceLocatorSetup = new DefaultServiceLocatorSetup();

        /// <summary>
        /// Gets the assembly search directories. When resolving assembly references, these directories
        /// will be searched.
        /// </summary>
        public string[] AssemblySearchDirectories { get; set; } = new string[0];

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public Logger Logger { get; set; }

        /// <summary>
        /// Gets or sets the scope of Smocks: should it rewrite only direct references
        /// or rewrite any loaded assembly.
        /// </summary>
        public Scope Scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        /// <summary>
        /// Gets or sets the service locator setup.
        /// </summary>
        public IServiceLocatorSetup ServiceLocatorSetup
        {
            get
            {
                return _serviceLocatorSetup;
            }

            set
            {
                ArgumentChecker.NotNull(value, () => value);
                _serviceLocatorSetup = value;
            }
        }
    }
}