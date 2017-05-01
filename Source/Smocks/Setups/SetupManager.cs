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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Smocks.Utility;

namespace Smocks.Setups
{
    internal class SetupManager : ISetupManager
    {
        private readonly IExpressionHelper _expressionHelper;
        private ImmutableList<IInternalSetup> _setups = new ImmutableList<IInternalSetup>();

        internal SetupManager(IExpressionHelper expressionHelper)
        {
            ArgumentChecker.NotNull(expressionHelper, () => expressionHelper);

            _expressionHelper = expressionHelper;
        }

        public ISetup Create(Expression<Action> expression)
        {
            MethodCallInfo methodCall = _expressionHelper.GetMethod(expression);

            if (methodCall == null)
            {
                throw new ArgumentException("Not a method", nameof(expression));
            }

            var setup = new Setup(methodCall);
            AddSetup(setup);

            return setup;
        }

        public ISetup<TReturnValue> Create<TReturnValue>(Expression<Func<TReturnValue>> expression)
        {
            MethodCallInfo methodCall = _expressionHelper.GetPropertyGetCall(expression) ??
                _expressionHelper.GetMethod(expression);

            if (methodCall == null)
            {
                throw new ArgumentException("Not a method or property", nameof(expression));
            }

            var setup = new Setup<TReturnValue>(methodCall);
            AddSetup(setup);

            return setup;
        }

        public IEnumerator<IInternalSetup> GetEnumerator()
        {
            return _setups.GetEnumerator();
        }

        public ReadOnlyCollection<IInternalSetup> GetSetupsForMethod(MethodBase target)
        {
            return _setups
                .Where(setup => setup.MethodCall.Method == target)
                .ToList()
                .AsReadOnly();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void AddSetup(IInternalSetup setup)
        {
            // Standard pattern for thread-safe adding to an immutable list:
            // Take the current list, create the new list with the added item,
            // then try to atomically swap the new list with the old list, checking
            // that it hasn't changed in the meantime. If we cannot swap because the list
            // has changed in the meantime, retry. Repeat infinitely.
            while (true)
            {
                ImmutableList<IInternalSetup> snapshot = _setups;
                var added = snapshot.Add(setup);

                if (Interlocked.CompareExchange(ref _setups, added, snapshot) == snapshot)
                {
                    break;
                }
            }
        }
    }
}