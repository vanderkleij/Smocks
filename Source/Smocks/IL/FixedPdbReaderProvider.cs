using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;

namespace Smocks.IL
{
    /// <summary>
    /// This is a copy/paste of Cecil's <see cref="PdbReaderProvider"/> with a small bugfix.
    /// Will also submit a pull request to Cecil, but until that is merged we'll use this fix.
    /// </summary>
    /// <seealso cref="Mono.Cecil.Cil.ISymbolReaderProvider" />
    internal class FixedPdbReaderProvider : ISymbolReaderProvider
    {
        public ISymbolReader GetSymbolReader(ModuleDefinition module, string fileName)
        {
            return IsPortablePdb(GetPdbFileName(fileName))
                ? new PortablePdbReaderProvider().GetSymbolReader(module, fileName)
                : new NativePdbReaderProvider().GetSymbolReader(module, fileName);
        }

        public ISymbolReader GetSymbolReader(ModuleDefinition module, Stream symbolStream)
        {
            return IsPortablePdb(symbolStream)
                ? new PortablePdbReaderProvider().GetSymbolReader(module, symbolStream)
                : new NativePdbReaderProvider().GetSymbolReader(module, symbolStream);
        }

        private static string GetPdbFileName(string assemblyFileName)
        {
            return Path.ChangeExtension(assemblyFileName, ".pdb");
        }

        private static bool IsPortablePdb(string fileName)
        {
            // Here's the fix: Cecil's provider uses FileShare.None instead of FileShare.Read.
            // In our case, the PDB is typically already open (for reading), causing this to fail.
            // Since we're just reading some bytes from the PDB, I don't see why we wouldn't allow
            // others to simultaneously read from the stream. 
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                return IsPortablePdb(file);
        }

        private static bool IsPortablePdb(Stream stream)
        {
            const uint ppdbSignature = 0x424a5342;

            var position = stream.Position;
            try
            {
                var reader = new BinaryReader(stream);
                return reader.ReadUInt32() == ppdbSignature;
            }
            finally
            {
                stream.Position = position;
            }
        }
    }
}