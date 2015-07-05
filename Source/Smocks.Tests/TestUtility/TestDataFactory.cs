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
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.Setups;
using Smocks.Utility;

namespace Smocks.Tests.TestUtility
{
    [ExcludeFromCodeCoverage]
    internal static class TestDataFactory
    {
        private static readonly ExpressionHelper ExpressionHelper = new ExpressionHelper();

        internal static Instruction CreateInlineFieldInstruction()
        {
            FieldDefinition field = new FieldDefinition("Test", FieldAttributes.Public,
                CecilUtility.Import(typeof(object)));

            return Instruction.Create(OpCodes.Ldfld, field);
        }

        internal static MethodCallInfo CreateMethodCallInfo()
        {
            return CreateMethodCallInfo(() => Console.WriteLine());
        }

        internal static MethodCallInfo CreateMethodCallInfo(Expression<Action> expression)
        {
            return ExpressionHelper.GetMethod(expression);
        }

        internal static ModuleDefinition CreateModuleDefinition()
        {
            return ModuleDefinition.CreateModule("Dummy", ModuleKind.Dll);
        }

        internal static ParameterDefinition CreateParameterDefinition(int index)
        {
            var method = CecilUtility.Import(ReflectionUtility.GetMethod(() => int.Parse(string.Empty))).Resolve();

            ParameterDefinition parameter = null;

            for (int i = 0; i <= index; ++i)
            {
                parameter = new ParameterDefinition(CecilUtility.Import(typeof(object)));
                method.Parameters.Add(parameter);
            }

            return parameter;
        }

        internal static ReadOnlyCollection<IInternalSetup> CreateSetups(params Expression<Action>[] expressions)
        {
            return expressions
                .Select(expression => new Setup(ExpressionHelper.GetMethod(expression)))
                .Cast<IInternalSetup>()
                .ToList()
                .AsReadOnly();
        }

        internal static SetupTarget CreateSetupTarget(Expression<Action> expression)
        {
            var method = ExpressionHelper.GetMethod(expression).Method;
            return new SetupTarget(expression, method);
        }
    }
}