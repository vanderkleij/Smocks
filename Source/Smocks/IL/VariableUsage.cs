using Mono.Cecil.Cil;

namespace Smocks.IL
{
    /// <summary>
    /// Represent a usage of a variable.
    /// </summary>
    internal class VariableUsage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableUsage" /> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="instruction">The instruction.</param>
        /// <param name="operation">The operation: read or write.</param>
        public VariableUsage(int index, Instruction instruction, VariableOperation operation)
        {
            Operation = operation;
            Index = index;
            Instruction = instruction;
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the operation.
        /// </summary>
        public VariableOperation Operation { get; private set; }

        /// <summary>
        /// Gets the instruction.
        /// </summary>
        public Instruction Instruction { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", Operation, Index);
        }
    }
}