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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Smocks.Utility;

namespace Smocks.Setups
{
    internal class SetupMatcher : ISetupMatcher
    {
        private readonly IArgumentMatcher _argumentMatcher;
        private readonly ISetupManager _setupManager;
        private readonly ITargetMatcher _targetMatcher;

        internal SetupMatcher(ISetupManager setupManager,
            ITargetMatcher targetMatcher,
            IArgumentMatcher argumentMatcher)
        {
            ArgumentChecker.NotNull(setupManager, () => setupManager);
            ArgumentChecker.NotNull(targetMatcher, () => targetMatcher);
            ArgumentChecker.NotNull(argumentMatcher, () => argumentMatcher);

            _setupManager = setupManager;
            _targetMatcher = targetMatcher;
            _argumentMatcher = argumentMatcher;
        }

        public IInternalSetup GetBestMatchingSetup(MethodBase method, object[] arguments)
        {
            ReadOnlyCollection<IInternalSetup> setups = _setupManager.GetSetupsForMethod(method);

            // Reverse so that the last setup that matches is selected.
            return setups.Reverse().FirstOrDefault(setup => ArgumentsMatch(method, setup.MethodCall.Arguments, arguments));
        }

        private static IEnumerable<Expression> ExpandArrayExpression(Expression expression)
        {
            return ((NewArrayExpression)expression).Expressions;
        }

        private static IEnumerable<object> ExpandObjectArray(object array)
        {
            return ((IEnumerable)array).Cast<object>();
        }

        private static IEnumerable<T> FilterArguments<T>(
            MethodBase method,
            IEnumerable<T> items,
            Func<T, IEnumerable<T>> arrayExpander)
        {
            var parameters = method.GetParameters();

            int i = 0;
            foreach (var item in items)
            {
                // We skip out/ref parameters as these don't have to be matched.
                bool isOutParameter = i < parameters.Length && parameters[i].IsOut;
                if (!isOutParameter)
                {
                    // Params arrays aren't actually arrays from our perspective: we should treat each
                    // element in them as a single argument and match those individually, instead
                    // of checking whether the whole array reference is the same. We therefore
                    // expand params arrays to their contents here.
                    if (IsParamsArray(parameters[i]))
                    {
                        foreach (var expandedItem in arrayExpander(item))
                        {
                            yield return expandedItem;
                        }
                    }
                    else
                    {
                        yield return item;
                    }
                }

                ++i;
            }
        }

        private static bool IsParamsArray(ParameterInfo parameterInfo)
        {
            return parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), true).Any();
        }

        private bool ArgumentsMatch(MethodBase method,
            ReadOnlyCollection<Expression> setupArguments, object[] actualArguments)
        {
            if (actualArguments.Length == 0)
            {
                return true;
            }

            int itemsToSkip = 0;

            if (!method.IsStatic)
            {
                itemsToSkip = 1;

                if (!_targetMatcher.IsMatch(method.DeclaringType, setupArguments[0], actualArguments[0]))
                {
                    return false;
                }
            }

            var filteredExpressions = FilterArguments(method, setupArguments.Skip(itemsToSkip), ExpandArrayExpression);
            var filteredObjects = FilterArguments(method, actualArguments.Skip(itemsToSkip), ExpandObjectArray);
            bool argumentsMatch = actualArguments.Length == itemsToSkip || _argumentMatcher.IsMatch(filteredExpressions, filteredObjects);

            return argumentsMatch;
        }
    }
}