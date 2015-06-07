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
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.IL.Resolvers;
using Smocks.IL.Visitors;
using Smocks.Injection;
using Smocks.Utility;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace Smocks.IL
{
    internal class DynamicMethodCompiler : IInstructionsCompiler
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeResolver _typeResolver;

        internal DynamicMethodCompiler(
            IServiceLocator serviceLocator,
            ITypeResolver typeResolver)
        {
            ArgumentChecker.NotNull(serviceLocator, () => serviceLocator);
            ArgumentChecker.NotNull(typeResolver, () => typeResolver);

            _serviceLocator = serviceLocator;
            _typeResolver = typeResolver;
        }

        public ICompiledMethod<T> Compile<T>(TypeReference[] parameters, IEnumerable<Instruction> instructions, IEnumerable<VariableDefinition> variables)
        {
            Type[] resolvedParameter = parameters.Select(_typeResolver.Resolve).ToArray();
            DynamicMethod method = new DynamicMethod("Execute", typeof(T), resolvedParameter, true);

            IILGenerator generator = new ILGeneratorWrapper(method.GetILGenerator());

            Dictionary<VariableReference, LocalBuilder> locals = AddVariables(generator, variables);
            AddInstructions(generator, instructions, locals);

            return new CompiledDynamicMethod<T>(method);
        }

        private void AddInstructions(IILGenerator generator, IEnumerable<Instruction> instructions,
            Dictionary<VariableReference, LocalBuilder> locals)
        {
            IInstructionVisitor visitor = new ILGeneratorInstructionVisitor(_serviceLocator, generator, locals);

            foreach (var instruction in instructions)
            {
                instruction.Accept(visitor);
            }

            generator.Emit(OpCodes.Ret);
        }

        private Dictionary<VariableReference, LocalBuilder> AddVariables(IILGenerator generator,
            IEnumerable<VariableReference> variables)
        {
            return variables.ToDictionary(
                variable => variable,
                variable => generator.DeclareLocal(_typeResolver.Resolve(variable.VariableType)));
        }
    }
}