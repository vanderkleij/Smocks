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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Smocks.Utility;

namespace Smocks.Setups
{
    internal class ArgumentMatcher : IArgumentMatcher
    {
        private readonly IExpressionHelper _expressionHelper;

        internal ArgumentMatcher(IExpressionHelper expressionHelper)
        {
            ArgumentChecker.NotNull(expressionHelper, () => expressionHelper);

            _expressionHelper = expressionHelper;
        }

        public bool IsMatch(IEnumerable<Expression> setupArguments,
            IEnumerable<object> actualArguments)
        {
            return IsMatch(setupArguments.ToList().AsReadOnly(),
                actualArguments.ToList().AsReadOnly());
        }

        private bool IsMatch(ReadOnlyCollection<Expression> setupArguments,
            ReadOnlyCollection<object> actualArguments)
        {
            for (int i = 0; i < setupArguments.Count; ++i)
            {
                if (_expressionHelper.IsUnconditionalAny(setupArguments[i]))
                {
                    continue;
                }

                object setupValue = _expressionHelper.GetValue(setupArguments[i]);
                object actualValue = actualArguments[i];

                if (!Equals(setupValue, actualValue))
                {
                    return false;
                }
            }

            return true;
        }
    }
}