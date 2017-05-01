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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks.IL
{
    internal class EventRewriteTarget : IRewriteTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventRewriteTarget"/> class.
        /// </summary>
        /// <param name="addMethod">The add method.</param>
        /// <param name="removeMethod">The remove method.</param>
        /// <param name="eventHandlerType">Type of the event handler.</param>
        public EventRewriteTarget(MethodBase addMethod, MethodBase removeMethod, Type eventHandlerType)
        {
            ArgumentChecker.NotNull(removeMethod, nameof(removeMethod));
            ArgumentChecker.NotNull(addMethod, nameof(addMethod));
            ArgumentChecker.NotNull(eventHandlerType, nameof(eventHandlerType));

            AddMethod = addMethod;
            RemoveMethod = removeMethod;
            EventHandlerType = eventHandlerType;
            Methods = new List<MethodBase> { AddMethod, RemoveMethod }.AsReadOnly();
        }

        /// <summary>
        /// Gets the add method.
        /// </summary>
        public MethodBase AddMethod { get; }

        /// <summary>
        /// Gets the type of the event handler.
        /// </summary>
        public Type EventHandlerType { get; }

        /// <summary>
        /// Gets a value indicating whether the target is an event accessor method.
        /// </summary>
        public bool IsEvent => true;

        /// <summary>
        /// Gets the methods that should be rewritten.
        /// </summary>
        public ReadOnlyCollection<MethodBase> Methods { get; }

        /// <summary>
        /// Gets the remove method.
        /// </summary>
        public MethodBase RemoveMethod { get; }
    }
}