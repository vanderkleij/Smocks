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
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Smocks.Setups;

namespace Smocks.IL
{
    internal class RewriteContext
    {
        public RewriteContext(Configuration configuration, ILProcessor processor,
            Instruction originalInstruction, List<IRewriteTarget> targets, MethodReference method)
        {
            Variables = new List<VariableDefinition>();
            Configuration = configuration;
            Processor = processor;
            OriginalInstruction = originalInstruction;
            Targets = targets;
            Method = method;
        }

        public Configuration Configuration { get; }

        public MethodReference Method { get; }

        public Instruction OriginalInstruction { get; }

        public ILProcessor Processor { get; }

        public List<IRewriteTarget> Targets { get; }

        public List<VariableDefinition> Variables { get; }

        public Instruction CurrentPosition => OriginalInstruction.Previous;

        public void InsertAfter(Instruction target, IEnumerable<Instruction> instructions)
        {
            foreach (var instruction in instructions.Reverse())
            {
                Processor.InsertAfter(target, instruction);
            }
        }

        public Instruction Insert(Instruction instruction)
        {
            Processor.InsertBefore(OriginalInstruction, instruction);
            return instruction;
        }

        public void AddVariables(IEnumerable<VariableDefinition> variables)
        {
            foreach (var variable in variables)
            {
                Processor.Body.Variables.Add(variable);
            }
        }
    }
}