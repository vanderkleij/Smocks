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
            return setups.Reverse().FirstOrDefault(setup =>
                ArgumentsMatch(method, setup.MethodCall.Arguments, arguments));
        }

        private static IEnumerable<T> FilterArguments<T>(MethodBase method, IEnumerable<T> items)
        {
            var parameters = method.GetParameters();

            // We skip out parameters as these don't have to be matched.
            int i = 0;
            foreach (var item in items)
            {
                if (parameters.Length <= i || parameters[i].IsOut == false)
                {
                    yield return item;
                }

                ++i;
            }
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

            bool argumentsMatch = actualArguments.Length == itemsToSkip || _argumentMatcher.IsMatch(
                                      FilterArguments(method, setupArguments.Skip(itemsToSkip)),
                                      FilterArguments(method, actualArguments.Skip(itemsToSkip)));

            return argumentsMatch;
        }
    }
}