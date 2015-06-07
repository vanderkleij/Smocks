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
using System.Linq;

namespace Smocks.Utility
{
    /// <summary>
    /// A very naive implementation of aa immutable list that is
    /// very expensive to add to, but cheap to read from.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    internal class ImmutableList<T> : IEnumerable<T>
    {
        private static readonly T[] EmptyArray = new T[0];
        private readonly T[] _items;

        public ImmutableList()
        {
            _items = EmptyArray;
        }

        private ImmutableList(T[] items)
        {
            _items = items;
        }

        public int Count
        {
            get { return _items.Length; }
        }

        public static ImmutableList<T> Create(params T[] items)
        {
            return new ImmutableList<T>(items);
        }

        public ImmutableList<T> Add(T value)
        {
            var items = new T[_items.Length + 1];
            Array.Copy(_items, items, _items.Length);
            items[_items.Length] = value;

            return new ImmutableList<T>(items);
        }

        public ImmutableList<T> AddRange(IEnumerable<T> values)
        {
            var newItems = values.ToArray();

            var items = new T[_items.Length + newItems.Length];
            Array.Copy(_items, items, _items.Length);
            Array.Copy(newItems, 0, items, _items.Length, newItems.Length);

            return new ImmutableList<T>(items);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(_items);
        }

        internal class Enumerator : IEnumerator<T>
        {
            private readonly T[] _items;
            private T _current;
            private int _index = 0;

            public Enumerator(T[] items)
            {
                _items = items;
            }

            public T Current
            {
                get
                {
                    return _current;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < _items.Length)
                {
                    _current = _items[_index++];
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                _index = 0;
                _current = default(T);
            }
        }
    }
}