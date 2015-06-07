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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Smocks.Utility
{
    internal class MethodCallInfo : IEquatable<MethodCallInfo>
    {
        internal MethodCallInfo(MethodBase method)
            : this(method, new List<Expression>().AsReadOnly())
        {
        }

        internal MethodCallInfo(MethodBase method, params Expression[] arguments)
            : this(method, arguments.ToList().AsReadOnly())
        {
        }

        internal MethodCallInfo(MethodBase method, ReadOnlyCollection<Expression> arguments)
        {
            Method = method;
            Arguments = arguments;
        }

        public ReadOnlyCollection<Expression> Arguments { get; set; }

        public MethodBase Method { get; set; }

        public bool Equals(MethodCallInfo other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Method, other.Method);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as MethodCallInfo);
        }

        public override int GetHashCode()
        {
            return Method != null ? Method.GetHashCode() : 0;
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Method.ToString();
        }
    }
}