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
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Smocks.Injection;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks
{
    /// <summary>
    /// The context for a Smocks session. The user can use the context to configure
    /// setups and verify expectations.
    /// </summary>
    [Serializable]
    public class SmocksContext : ISmocksContext, ISerializable
    {
        private readonly IInvocationTracker _invocationTracker;
        private readonly EventInterceptor _eventInterceptor;
        private readonly ISetupManager _setupManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmocksContext" /> class.
        /// </summary>
        /// <param name="setupManager">The setup manager.</param>
        /// <param name="invocationTracker">The invocation tracker.</param>
        /// <param name="eventInterceptor">The event interceptor.</param>
        internal SmocksContext(ISetupManager setupManager, IInvocationTracker invocationTracker, EventInterceptor eventInterceptor)
        {
            ArgumentChecker.NotNull(setupManager, nameof(setupManager));
            ArgumentChecker.NotNull(invocationTracker, nameof(invocationTracker));
            ArgumentChecker.NotNull(eventInterceptor, nameof(eventInterceptor));

            _setupManager = setupManager;
            _invocationTracker = invocationTracker;
            _eventInterceptor = eventInterceptor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmocksContext"/> class by
        /// deserializing from a <see cref="SerializationInfo "/>.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected SmocksContext(SerializationInfo info, StreamingContext context)
            : this(ServiceLocator.Instance.Resolve<ISetupManager>(),
                   ServiceLocator.Instance.Resolve<IInvocationTracker>(),
                   ServiceLocator.Instance.Resolve<EventInterceptor>())
        {
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// Creates a setup for the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// A setup.
        /// </returns>
        public ISetup Setup(Expression<Action> expression)
        {
            return _setupManager.Create(expression);
        }

        /// <summary>
        /// Creates a setup for the specified expression.
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// A setup.
        /// </returns>
        public ISetup<TReturnValue> Setup<TReturnValue>(Expression<Func<TReturnValue>> expression)
        {
            return _setupManager.Create(expression);
        }

        /// <summary>
        /// Verifies the expectations configured for the setups.
        /// </summary>
        public void Verify()
        {
            _invocationTracker.Verify();
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="addExpression">A lambda pointing to the add handler of the event to raise.</param>
        /// <param name="removeExpression">A lambda pointing to the remove handler of the event to raise.</param>
        /// <param name="eventArgs">The instance containing the event data.</param>
        /// <returns>
        /// The value returned from invoking the event, if any.
        /// </returns>
        public object Raise(Action addExpression, Action removeExpression, EventArgs eventArgs)
        { 
            return _eventInterceptor.Raise(addExpression, removeExpression, eventArgs);
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="addExpression">A lambda pointing to the add handler of the event to raise.</param>
        /// <param name="removeExpression">A lambda pointing to the remove handler of the event to raise.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// The value returned from invoking the event, if any.
        /// </returns>
        public object Raise(Action addExpression, Action removeExpression, params object[] arguments)
        {
            return _eventInterceptor.Raise(addExpression, removeExpression, arguments);
        }
    }
}