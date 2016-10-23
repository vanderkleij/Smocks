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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Smocks.Exceptions;
using Smocks.Utility;

namespace Smocks.Setups
{
    internal class InvocationTracker : IInvocationTracker
    {
        private readonly ConcurrentDictionary<MethodBase, List<Invocation>> _invocations =
            new ConcurrentDictionary<MethodBase, List<Invocation>>();

        private readonly ISetupManager _setupManager;

        internal InvocationTracker(ISetupManager setupManager)
        {
            ArgumentChecker.NotNull(setupManager, () => setupManager);

            _setupManager = setupManager;
        }

        public void Track(MethodBase method, object[] arguments, IInternalSetup setup)
        {
            Invocation invocation = new Invocation(setup, arguments);

            _invocations.AddOrUpdate(
                method,
                new List<Invocation> { invocation },
                (key, existing) =>
                {
                    existing.Add(invocation);
                    return existing;
                });
        }

        public void Verify()
        {
            List<IInternalSetup> notMatchedSetups = new List<IInternalSetup>();

            foreach (IInternalSetup setup in _setupManager.Where(setup => setup.Verify))
            {
                List<Invocation> invocationList;
                if (_invocations.TryGetValue(setup.MethodCall.Method, out invocationList))
                {
                    if (invocationList.Any(invocation => invocation.MatchedSetup == setup))
                    {
                        continue;
                    }
                }

                notMatchedSetups.Add(setup);
            }

            if (notMatchedSetups.Count > 0)
            {
                throw new VerificationException(notMatchedSetups.ToArray());
            }
        }

        private class Invocation
        {
            private readonly object[] _arguments;

            public Invocation(IInternalSetup matchedSetup, object[] arguments)
            {
                MatchedSetup = matchedSetup;
                _arguments = arguments;
            }

            public IInternalSetup MatchedSetup { get; }

            [ExcludeFromCodeCoverage]
            public override string ToString()
            {
                return string.Join(", ", _arguments ?? new object[0]);
            }
        }
    }
}