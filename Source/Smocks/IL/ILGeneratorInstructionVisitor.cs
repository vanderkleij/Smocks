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
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.IL.Resolvers;
using Smocks.IL.Visitors;
using Smocks.Injection;
using Smocks.Utility;
using OperandType = Mono.Cecil.Cil.OperandType;

namespace Smocks.IL
{
    /// <summary>
    /// This visitor for Mono Cecil instructions takes an (.NET) IL.Emit generator
    /// and emits instructions equal to the Cecil instructions visited.
    /// This way, it converts code in Mono Cecil format to IL.Emit format.
    /// </summary>
    internal class ILGeneratorInstructionVisitor : IInstructionVisitor
    {
        private readonly IFieldResolver _fieldResolver;
        private readonly IILGenerator _generator;

        private readonly Dictionary<Instruction, Label> _instructionLabels
            = new Dictionary<Instruction, Label>();

        private readonly IDictionary<VariableReference, LocalBuilder> _locals;
        private readonly IMethodResolver _methodResolver;
        private readonly IOpCodeMapper _opCodeMapper;
        private readonly ITypeResolver _typeResolver;

        internal ILGeneratorInstructionVisitor(IILGenerator generator)
            : this(ServiceLocator.Default, generator, new Dictionary<VariableReference, LocalBuilder>())
        {
        }

        internal ILGeneratorInstructionVisitor(
                IServiceLocator serviceLocator,
                IILGenerator generator,
                IDictionary<VariableReference, LocalBuilder> locals)
            : this(
                serviceLocator,
                generator,
                locals,
                serviceLocator.Resolve<ITypeResolver>(),
                serviceLocator.Resolve<IOpCodeMapper>())
        {
        }

        internal ILGeneratorInstructionVisitor(
                IServiceLocator serviceLocator,
                IILGenerator generator,
                IDictionary<VariableReference, LocalBuilder> locals,
                ITypeResolver typeResolver,
                IOpCodeMapper opCodeMapper)
            : this(generator,
                locals,
                typeResolver,
                serviceLocator.Resolve<IMethodResolver>(),
                serviceLocator.Resolve<IFieldResolver>(),
                opCodeMapper)
        {
        }

        internal ILGeneratorInstructionVisitor(IILGenerator generator,
            IDictionary<VariableReference, LocalBuilder> locals,
            ITypeResolver typeResolver,
            IMethodResolver methodResolver,
            IFieldResolver fieldResolver,
            IOpCodeMapper opCodeMapper)
        {
            ArgumentChecker.NotNull(generator, () => generator);
            ArgumentChecker.NotNull(locals, () => locals);
            ArgumentChecker.NotNull(typeResolver, () => typeResolver);
            ArgumentChecker.NotNull(methodResolver, () => methodResolver);
            ArgumentChecker.NotNull(fieldResolver, () => fieldResolver);
            ArgumentChecker.NotNull(opCodeMapper, () => opCodeMapper);

            _generator = generator;
            _locals = locals;
            _typeResolver = typeResolver;
            _methodResolver = methodResolver;
            _fieldResolver = fieldResolver;
            _opCodeMapper = opCodeMapper;
        }

        public Unit Visit(Instruction instruction)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode));
            return Unit.Value;
        }

        public Unit VisitInlineArg(Instruction instruction, ParameterReference operand)
        {
            MarkLabel(instruction);
            var opCode = _opCodeMapper.Map(instruction.OpCode);

            if (instruction.OpCode.OperandType == OperandType.InlineArg)
            {
                _generator.Emit(opCode, (ushort)operand.Index);
            }
            else
            {
                _generator.Emit(opCode, (byte)operand.Index);
            }

            return Unit.Value;
        }

        public Unit VisitInlineBrTarget(Instruction instruction, Instruction operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), GetOrCreateLabel(operand));
            return Unit.Value;
        }

        public Unit VisitInlineField(Instruction instruction, FieldReference operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), _fieldResolver.Resolve(operand));
            return Unit.Value;
        }

        public Unit VisitInlineInteger(Instruction instruction, long operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), operand);
            return Unit.Value;
        }

        public Unit VisitInlineInteger(Instruction instruction, int operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), operand);
            return Unit.Value;
        }

        public Unit VisitInlineInteger(Instruction instruction, sbyte operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), operand);
            return Unit.Value;
        }

        public Unit VisitInlineMethod(Instruction instruction, MethodReference operand)
        {
            MarkLabel(instruction);

            MethodBase method = _methodResolver.Resolve(operand);

            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo != null)
            {
                _generator.Emit(_opCodeMapper.Map(instruction.OpCode), methodInfo);
            }
            else
            {
                ConstructorInfo constructor = (ConstructorInfo)method;
                _generator.Emit(_opCodeMapper.Map(instruction.OpCode), constructor);
            }

            return Unit.Value;
        }

        public Unit VisitInlineR(Instruction instruction, float operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), operand);
            return Unit.Value;
        }

        public Unit VisitInlineR8(Instruction instruction, double operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), operand);
            return Unit.Value;
        }

        public Unit VisitInlineSig(Instruction instruction, CallSite operand)
        {
            MarkLabel(instruction);
            throw new NotImplementedException();
        }

        public Unit VisitInlineString(Instruction instruction, string operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), operand);
            return Unit.Value;
        }

        public Unit VisitInlineSwitch(Instruction instruction, Instruction[] operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), operand.Select(GetOrCreateLabel).ToArray());
            return Unit.Value;
        }

        public Unit VisitInlineTok(Instruction instruction, TypeReference typeReference)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), _typeResolver.Resolve(typeReference));
            return Unit.Value;
        }

        public Unit VisitInlineTok(Instruction instruction, FieldReference fieldReference)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), _fieldResolver.Resolve(fieldReference));
            return Unit.Value;
        }

        public Unit VisitInlineTok(Instruction instruction, MethodReference methodReference)
        {
            MarkLabel(instruction);

            MethodBase method = _methodResolver.Resolve(methodReference);

            MethodInfo methodInfo = method as MethodInfo;

            if (methodInfo != null)
            {
                _generator.Emit(_opCodeMapper.Map(instruction.OpCode), methodInfo);
            }
            else
            {
                _generator.Emit(_opCodeMapper.Map(instruction.OpCode), (ConstructorInfo)method);
            }

            return Unit.Value;
        }

        public Unit VisitInlineType(Instruction instruction, TypeReference operand)
        {
            MarkLabel(instruction);
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), _typeResolver.Resolve(operand));
            return Unit.Value;
        }

        public Unit VisitInlineVar(Instruction instruction, VariableReference operand)
        {
            MarkLabel(instruction);

            var local = _locals[operand];
            _generator.Emit(_opCodeMapper.Map(instruction.OpCode), local);
            return Unit.Value;
        }

        private Label GetOrCreateLabel(Instruction instruction)
        {
            Label result;

            if (!_instructionLabels.TryGetValue(instruction, out result))
            {
                result = _generator.DefineLabel();
                _instructionLabels.Add(instruction, result);
            }

            return result;
        }

        private void MarkLabel(Instruction instruction)
        {
            Label label = GetOrCreateLabel(instruction);
            _generator.MarkLabel(label);
        }
    }
}