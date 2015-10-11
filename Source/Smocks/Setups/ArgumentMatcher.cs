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
        private readonly IItIsMatcher _itIsMatcher;

        internal ArgumentMatcher(IExpressionHelper expressionHelper, IItIsMatcher itIsMatcher)
        {
            ArgumentChecker.NotNull(expressionHelper, () => expressionHelper);
            ArgumentChecker.NotNull(itIsMatcher, () => itIsMatcher);

            _expressionHelper = expressionHelper;
            _itIsMatcher = itIsMatcher;
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
                // Skip if the setup argument is It.IsAny<T>.
                // We match the call by name, so that both Smock's and Moq's It class can be used.
                if (_expressionHelper.IsMethodInvocation(setupArguments[i], "It", "IsAny", 0))
                {
                    continue;
                }

                object actualValue = actualArguments[i];

                // Check if the setup argument is It.Is<T>(...).
                bool isItIs = _expressionHelper.IsMethodInvocation(setupArguments[i], "It", "Is", 1);
                if (isItIs)
                {
                    if (!_itIsMatcher.ItIsMatch(setupArguments[i], actualValue))
                    {
                        return false;
                    }
                }
                else
                {
                    object setupValue = _expressionHelper.GetValue(setupArguments[i]);

                    if (!Equals(setupValue, actualValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}