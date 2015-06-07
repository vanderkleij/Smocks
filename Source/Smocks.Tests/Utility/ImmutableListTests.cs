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

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Smocks.Utility;

namespace Smocks.Tests.Utility
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ImmutableListTests
    {
        [TestCase]
        public void Add_AddsItemAfterExistingItems()
        {
            var subject = ImmutableList<int>.Create(1, 2, 3);
            var result = subject.Add(4).ToList();

            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4 }));
        }

        [TestCase]
        public void Add_ReturnsNewListWithNewItemAppended()
        {
            var subject = new ImmutableList<int>();
            var result = subject.Add(12);

            Assert.AreEqual(0, subject.Count);
            Assert.AreEqual(1, result.Count);
        }

        [TestCase]
        public void AddRange_AddsItemsAfterExistingItems()
        {
            var subject = ImmutableList<int>.Create(1, 2, 3);
            var result = subject.AddRange(Enumerable.Range(4, 3)).ToList();

            Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [TestCase]
        public void AddRange_ReturnsNewListWithNewItemsAppended()
        {
            var subject = new ImmutableList<int>();
            var result = subject.AddRange(Enumerable.Range(1, 10));

            Assert.AreEqual(0, subject.Count);
            Assert.AreEqual(10, result.Count);
        }
    }
}