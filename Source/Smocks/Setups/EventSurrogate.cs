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

namespace Smocks.Setups
{
    /// <summary>
    /// A surrogate for an actual event: it tracks subscriptions to
    /// an event so that raising the event can be faked.
    /// </summary>
    internal abstract class EventSurrogate
    {
        public abstract void Subscribe(Delegate subscriber);

        public abstract void Unsubscribe(Delegate subscriber);

        public abstract object Raise(object[] arguments);
    }

    /// <summary>
    /// A surrogate for an actual event: it tracks subscriptions to
    /// an event so that raising the event can be faked.
    /// </summary>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    internal class EventSurrogate<TEventHandler> : EventSurrogate
        where TEventHandler : class
    {
        private readonly object _lock = new object();
        private Delegate _delegate;

        public override void Subscribe(Delegate subscriber)
        {
            if (!(subscriber is TEventHandler))
            {
                throw new ArgumentException("Invalid delegate type", nameof(subscriber));
            }

            lock (_lock)
            {
                _delegate = Delegate.Combine(_delegate, subscriber);
            }
        }

        public override void Unsubscribe(Delegate subscriber)
        {
            if (!(subscriber is TEventHandler))
            {
                throw new ArgumentException("Invalid delegate type", nameof(subscriber));
            }

            lock (_lock)
            {
                _delegate = Delegate.Remove(_delegate, subscriber);
            }
        }

        public override object Raise(object[] arguments)
        {
            return _delegate?.DynamicInvoke(arguments);
        }
    }
}