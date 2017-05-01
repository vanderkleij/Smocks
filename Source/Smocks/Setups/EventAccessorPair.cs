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
using Smocks.Utility;

namespace Smocks.Setups
{
    /// <summary>
    /// Stores the add and remove accessor methods of an event, 
    /// as well as the target instance in case of non-static events.
    /// The class is equatable, so that the add-remove accessor pair
    /// can be equality-checked against others.
    /// </summary>
    internal class EventAccessorPair : IEquatable<EventAccessorPair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAccessorPair" /> class.
        /// </summary>
        /// <param name="targetInstance">The target instance.</param>
        /// <param name="addAccessor">The add accessor.</param>
        /// <param name="removeAccessor">The remove accessor.</param>
        public EventAccessorPair(object targetInstance, MethodBase addAccessor, MethodBase removeAccessor)
        {
            ArgumentChecker.NotNull(addAccessor, nameof(addAccessor));
            ArgumentChecker.NotNull(removeAccessor, nameof(removeAccessor));

            TargetInstance = targetInstance;
            AddAccessor = addAccessor;
            RemoveAccessor = removeAccessor;
        }

        /// <summary>
        /// Gets the target instance.
        /// </summary>
        public object TargetInstance { get; }

        /// <summary>
        /// Gets the add accessor.
        /// </summary>
        public MethodBase AddAccessor { get; }

        /// <summary>
        /// Gets the remove accessor.
        /// </summary>
        public MethodBase RemoveAccessor { get; }

        public bool Equals(EventAccessorPair other)
        {
            return other != null && 
                   other.TargetInstance == TargetInstance &&
                   other.AddAccessor == AddAccessor && 
                   other.RemoveAccessor == RemoveAccessor;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EventAccessorPair);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + AddAccessor?.GetHashCode() ?? 0;
                hash = hash * 23 + RemoveAccessor?.GetHashCode() ?? 0;
                hash = hash * 23 + TargetInstance?.GetHashCode() ?? 0;
                return hash;
            }
        }
    }
}