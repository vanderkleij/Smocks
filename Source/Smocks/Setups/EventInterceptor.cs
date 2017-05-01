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
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Smocks.IL;
using Smocks.Injection;
using Smocks.Utility;

namespace Smocks.Setups
{
    /// <summary>
    /// This class is used by the internals of Smocks in the interception of
    /// event accessor method calls.
    /// </summary>
    public class EventInterceptor
    {
        private readonly IEventAccessorExtractor _eventAccessorExtractor;
        private readonly ConcurrentDictionary<EventAccessorPair, EventSurrogate> _events = new ConcurrentDictionary<EventAccessorPair, EventSurrogate>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInterceptor"/> class.
        /// </summary>
        /// <param name="eventAccessorExtractor">The event accessor extractor.</param>
        internal EventInterceptor(IEventAccessorExtractor eventAccessorExtractor)
        {
            ArgumentChecker.NotNull(eventAccessorExtractor, nameof(eventAccessorExtractor));

            _eventAccessorExtractor = eventAccessorExtractor;
        }

        /// <summary>
        /// Intercepts the specified void event method.
        /// </summary>
        /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
        /// <param name="arguments">The arguments provided in the original method call.</param>
        /// <param name="invokedMethod">The invoked method.</param>
        /// <param name="addMethod">The add accessor method of the event.</param>
        /// <param name="removeMethod">The remove accessor method of the event.</param>
        /// <returns>
        /// A result that specifies whether the method was intercepted.
        /// </returns>
        public static InterceptorResult InterceptVoidEvent<TEventHandler>(object[] arguments,
            MethodBase invokedMethod, MethodBase addMethod, MethodBase removeMethod) where TEventHandler : class
        {
            return ServiceLocator.Instance.Resolve<EventInterceptor>()
                .InterceptVoidEventMethod<TEventHandler>(arguments, invokedMethod, addMethod, removeMethod);
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="addAction">The action pointing to the add accessor.</param>
        /// <param name="removeAction">The action pointing to the remove accessor.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <returns>The value returned from the event subscriber(s), if any.</returns>
        public object Raise(Action addAction, Action removeAction, EventArgs eventArgs)
        {
            EventAccessorPair accessorPair = _eventAccessorExtractor.FindEventAccessor(addAction, removeAction);
            object sender = accessorPair.TargetInstance;

            return Raise(accessorPair, new[] { sender, eventArgs });
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="addAction">The action pointing to the add accessor.</param>
        /// <param name="removeAction">The action pointing to the remove accessor.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// The value returned from the event subscriber(s), if any.
        /// </returns>
        public object Raise(Action addAction, Action removeAction, object[] arguments)
        {
            EventAccessorPair accessorPair = _eventAccessorExtractor.FindEventAccessor(addAction, removeAction);

            return Raise(accessorPair, arguments);
        }

        /// <summary>
        /// Intercepts the specified void event method.
        /// </summary>
        /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
        /// <param name="arguments">The arguments provided in the original method call.</param>
        /// <param name="invokedMethod">The invoked method.</param>
        /// <param name="addMethod">The add accessor method.</param>
        /// <param name="removeMethod">The remove accessor method.</param>
        /// <returns>
        /// A result that specifies whether the method was intercepted.
        /// </returns>
        private InterceptorResult InterceptVoidEventMethod<TEventHandler>(object[] arguments, MethodBase invokedMethod,
            MethodBase addMethod, MethodBase removeMethod) where TEventHandler : class
        {
            object targetInstance = invokedMethod.IsStatic ? null : arguments[0];
            var key = new EventAccessorPair(targetInstance, addMethod, removeMethod);

            EventSurrogate surrogate = _events.GetOrAdd(key, _ => new EventSurrogate<TEventHandler>());
            Delegate subscriber = (Delegate)(invokedMethod.IsStatic ? arguments[0] : arguments[1]);

            if (invokedMethod == addMethod)
            {
                surrogate.Subscribe(subscriber);
            }
            else
            {
                surrogate.Unsubscribe(subscriber);
            }

            // Invoke the original event accessors too.
            return new InterceptorResult(false);
        }

        private object Raise(EventAccessorPair accessorPair, object[] arguments)
        {
            EventSurrogate surrogate;
            if (!_events.TryGetValue(accessorPair, out surrogate))
            {
                throw new ArgumentException("Could not find event surrogate for add/remove accessor pair");
            }

            return surrogate.Raise(arguments.ToArray());
        }
    }
}