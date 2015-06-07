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
            IEqualityComparer<ModuleReference> moduleComparer )
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