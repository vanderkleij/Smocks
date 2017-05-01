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
using NUnit.Framework;

namespace Smocks.Tests
{
    [TestFixture]
    public class EventsTests
    {
        [TestCase]
        public void Raise_EventWithReturnValue_ReturnsValueFromSubscriber()
        {
            Smock.Run(context =>
            {
                ClassWithEvents first = new ClassWithEvents();
                first.EventWithReturnValue += value => value.ToString();

                object result = context.Raise(() => first.EventWithReturnValue += null,
                    () => first.EventWithReturnValue -= null, 42);

                Assert.AreEqual("42", result);
            });
        }

        [TestCase]
        public void Raise_NonMatchingAddRemoveAccessor_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    context.Raise(() => Console.CancelKeyPress += null, () => ClassWithEvents.StaticEvent -= null, default(EventArgs));
                });
            });
        }

        [TestCase]
        public void Raise_NonMatchingTargets_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Smock.Run(context =>
                {
                    var first = new ClassWithEvents();
                    var second = new ClassWithEvents();

                    context.Raise(() => first.TheEvent += null, () => second.TheEvent -= null, default(EventArgs));
                });
            });
        }

        [TestCase]
        public void Raise_NonStaticEvent_OriginalEventInvokesSubscribers()
        {
            int invocationCount = 0;

            Smock.Run(context =>
            {
                ClassWithEvents instance = new ClassWithEvents();

                instance.TheEvent += (sender, args) => ++invocationCount;

                instance.RaiseTheEvent(EventArgs.Empty);
                context.Raise(() => instance.TheEvent += null, () => instance.TheEvent -= null, EventArgs.Empty);
            });

            Assert.AreEqual(2, invocationCount);
        }

        [TestCase]
        public void Raise_NonStaticEvent_PassesEventArgsAndSender()
        {
            Smock.Run(context =>
            {
                ClassWithEvents instance = new ClassWithEvents();

                EventArgs eventArgs = new EventArgs();

                EventArgs passedEventArgs = null;
                object passedSender = null;

                instance.TheEvent += (sender, args) => { passedEventArgs = args; passedSender = sender; };
                context.Raise(() => instance.TheEvent += null, () => instance.TheEvent -= null, eventArgs);

                Assert.AreSame(instance, passedSender);
                Assert.AreSame(eventArgs, passedEventArgs);
            });
        }

        [TestCase]
        public void Raise_NonStaticEvent_RaisesInvokedEventOnCorrectInstance()
        {
            Smock.Run(context =>
            {
                ClassWithEvents first = new ClassWithEvents();
                ClassWithEvents second = new ClassWithEvents();

                bool firstInvoked = false;
                first.TheEvent += (sender, args) => firstInvoked = true;

                bool secondInvoked = false;
                second.TheEvent += (sender, args) => secondInvoked = true;

                context.Raise(() => first.TheEvent += null, () => first.TheEvent -= null, EventArgs.Empty);

                Assert.IsFalse(secondInvoked);
                Assert.IsTrue(firstInvoked);
            });
        }

        [TestCase]
        public void Raise_StaticEvent_OriginalEventInvokesSubscribers()
        {
            int invocationCount = 0;

            Smock.Run(context =>
            {
                ClassWithEvents.StaticEvent += (sender, args) => ++invocationCount;
                ClassWithEvents.RaiseStaticEvent(EventArgs.Empty);
                context.Raise(() => ClassWithEvents.StaticEvent += null, () => ClassWithEvents.StaticEvent -= null, default(EventArgs));
            });

            Assert.AreEqual(2, invocationCount);
        }

        [TestCase]
        public void Raise_StaticEvent_PassesEventArgs()
        {
            Smock.Run(context =>
            {
                EventArgs eventArgs = new EventArgs();
                EventArgs passedEventArgs = null;

                ClassWithEvents.StaticEvent += (sender, args) => passedEventArgs = args;
                context.Raise(() => ClassWithEvents.StaticEvent += null, () => ClassWithEvents.StaticEvent -= null, eventArgs);

                Assert.AreSame(eventArgs, passedEventArgs);
            });
        }

        [TestCase]
        public void Raise_StaticEvent_RaisesInvokedEvent()
        {
            bool invoked = false;

            Smock.Run(context =>
            {
                Console.CancelKeyPress += (sender, args) => invoked = true;
                context.Raise(() => Console.CancelKeyPress += null, () => Console.CancelKeyPress -= null, default(EventArgs));
            });

            Assert.IsTrue(invoked);
        }

        [TestCase]
        public void Raise_UnsubcribeFromNonStaticEvent_DoesntInvokeUnsubscribedHandlerOnRaise()
        {
            bool invoked = false;

            Smock.Run(context =>
            {
                var instance = new ClassWithEvents();

                EventHandler handler = (sender, args) => invoked = true;

                instance.TheEvent += handler;
                instance.TheEvent -= handler;

                context.Raise(() => instance.TheEvent += null, () => instance.TheEvent -= null, default(EventArgs));
            });

            Assert.IsFalse(invoked);
        }

        [TestCase]
        public void Raise_UnsubcribeFromStaticEvent_DoesntInvokeUnsubscribedHandlerOnRaise()
        {
            bool invoked = false;

            Smock.Run(context =>
            {
                ConsoleCancelEventHandler handler = (sender, args) => invoked = true;

                Console.CancelKeyPress += handler;
                Console.CancelKeyPress -= handler;

                context.Raise(() => Console.CancelKeyPress += null, () => Console.CancelKeyPress -= null, default(EventArgs));
            });

            Assert.IsFalse(invoked);
        }

        private class ClassWithEvents
        {
            public static event EventHandler StaticEvent;

            public event Func<int, string> EventWithReturnValue;

            public event EventHandler TheEvent;

            public static void RaiseStaticEvent(EventArgs eventArgs)
            {
                StaticEvent?.Invoke(null, eventArgs);
            }

            public string RaiseEventWithReturnValue(int value)
            {
                return EventWithReturnValue?.Invoke(value);
            }

            public void RaiseTheEvent(EventArgs eventArgs)
            {
                TheEvent?.Invoke(this, eventArgs);
            }
        }
    }
}