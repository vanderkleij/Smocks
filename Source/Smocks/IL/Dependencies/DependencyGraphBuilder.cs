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
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.IL.Visitors;
using Smocks.Utility;

namespace Smocks.IL.Dependencies
{
    internal class DependencyGraphBuilder : IDependencyGraphBuilder
    {
        private readonly IMethodDisassembler _disassembler;
        private readonly IEqualityComparer<ModuleReference> _moduleComparer;

        internal DependencyGraphBuilder(IMethodDisassembler disassembler,
            IEqualityComparer<ModuleReference> moduleComparer)
        {
            ArgumentChecker.NotNull(disassembler, () => disassembler);
            ArgumentChecker.NotNull(moduleComparer, () => moduleComparer);

            _disassembler = disassembler;
            _moduleComparer = moduleComparer;
        }

        public DependencyGraph BuildGraphForMethod(MethodBase method)
        {
            var disassembled = _disassembler.Disassemble(method);
            var graph = new DependencyGraph(disassembled.ModuleDefinition, _moduleComparer);
            var visitor = new DependencyGraphInstructionVisitor(graph, _moduleComparer);

            foreach (var instruction in disassembled.Body.Instructions)
            {
                instruction.Accept(visitor);
            }

            return graph;
        }

        internal class DependencyGraphInstructionVisitor : InstructionVisitorBase<Unit>
        {
            private readonly IDependencyNodeContainer _container;
            private readonly IEqualityComparer<ModuleReference> _moduleComparer;

            public DependencyGraphInstructionVisitor(
                IDependencyNodeContainer container,
                IEqualityComparer<ModuleReference> moduleComparer)
            {
                _container = container;
                _moduleComparer = moduleComparer;
            }

            public override Unit Visit(Instruction instruction)
            {
                return Unit.Value;
            }

            public override Unit VisitMethod(Instruction instruction, MethodReference operand)
            {
                ProcessModule(operand.Resolve().Module);
                return Unit.Value;
            }

            public override Unit VisitType(Instruction instruction, TypeReference operand)
            {
                ProcessModule(operand.Resolve().Module);
                return Unit.Value;
            }

            private void ProcessModule(ModuleDefinition module)
            {
                var child = new DependencyGraphNode(module, _moduleComparer);

                if (_container.Nodes.Add(child))
                {
                    // TODO: build rest of the graph. For now we only need one level.
                }
            }
        }
    }
}