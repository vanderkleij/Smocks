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

using System.Linq.Expressions;
using Mono.Cecil.Cil;

namespace Smocks.IL
{
    internal interface IExpressionDecompiler<out TExpression>
    {
        /// <summary>
        /// Decompiles an expression that's on the stack at the specified
        /// instruction in the specified method. This is done by reversing from the
        /// specified instruction until the stack should be empty. The instructions
        /// can then be replayed from that point.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="instruction">The instruction.</param>
        /// <param name="target">The target of the method, if any.</param>
        /// <returns>The compiled expression.</returns>
        TExpression Decompile(MethodBody body, Instruction instruction, object target);

        /// <summary>
        /// Decompiles an expression that's on the stack at the specified
        /// instruction in the specified method. This is done by reversing from the
        /// specified instruction until the stack should be empty. The instructions
        /// can then be replayed from that point.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="instruction">The instruction.</param>
        /// <param name="target">The target of the method, if any.</param>
        /// <param name="expectedStackSize">The number of objects expected to be on the stack when we get to the instruction.</param>
        /// <param name="stackEntriesToSkip">The number of stack entries to skip.</param>
        /// <returns>
        /// The compiled expression.
        /// </returns>
        TExpression Decompile(MethodBody body, Instruction instruction, object target, int expectedStackSize, int stackEntriesToSkip);
    }
}