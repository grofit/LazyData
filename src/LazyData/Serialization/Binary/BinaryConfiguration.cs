using System.Collections.Generic;
using System.IO;

namespace LazyData.Serialization.Binary
{
    public class BinaryConfiguration : ISerializationConfiguration<BinaryWriter,BinaryReader>
    {
        public IEnumerable<IPrimitiveHandler<BinaryWriter, BinaryReader>> TypeHandlers { get; }

        public BinaryConfiguration(IEnumerable<IPrimitiveHandler<BinaryWriter, BinaryReader>> typeHandlers = null)
        {
            TypeHandlers = typeHandlers ?? new List<IPrimitiveHandler<BinaryWriter, BinaryReader>>();
        }
    }
}