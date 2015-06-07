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
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using NUnit.Framework;
using Smocks.IL;

namespace Smocks.Tests.IL
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class CompiledDynamicMethodTests
    {
        [TestCase]
        public void Constructor_MethodNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new CompiledDynamicMethod<string>(null));

            Assert.AreEqual("method", exception.ParamName);
        }

        [TestCase("Hello, world!")]
        public void Invoke_StaticMethod_PassesArgumentsAndReturnsReturnValue(string message)
        {
            var method = new DynamicMethod("Test", typeof(string), new[] { typeof(string) });
            var generator = method.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ret);

            var subject = new CompiledDynamicMethod<string>(method);
            var result = subject.Invoke(new object[] { message });

            Assert.AreEqual(message, result);
        }
    }
}