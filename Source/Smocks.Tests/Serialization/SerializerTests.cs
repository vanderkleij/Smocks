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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Smocks.Serialization;

namespace Smocks.Tests.Serialization
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SerializerTests
    {
        [TestCase("Test")]
        public void Deserialize_SetsFieldFromDictionary(string expected)
        {
            var subject = new Serializer();

            var values = CreateDictionary("PublicField", expected);
            var result = (SerializationTarget)subject.Deserialize(typeof(SerializationTarget), values);

            Assert.AreEqual(expected, result.PublicField);
        }

        [TestCase("Test")]
        public void Deserialize_String_GetsValueFromDictionary(string expected)
        {
            var subject = new Serializer();

            var values = new Dictionary<string, object>
            {
                ["Value"] = expected
            };

            var result = subject.Deserialize(typeof(string), values);

            Assert.AreEqual(expected, result);
        }

        [TestCase]
        public void Deserialize_TypeNull_ReturnsNull()
        {
            var subject = new Serializer();
            var result = subject.Deserialize(null, new Dictionary<string, object>());

            Assert.IsNull(result);
        }

        private Dictionary<string, object> CreateDictionary(string key, object value)
        {
            var result = new Dictionary<string, object>
            {
                [key] = value
            };

            return result;
        }

        private class SerializationTarget
        {
#pragma warning disable 0649
            public string PublicField;
#pragma warning restore 0649
        }
    }
}