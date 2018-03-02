using System.Collections.Generic;
using System.IO;

namespace LazyData.Serialization.Binary
{
    public class BinaryConfiguration : ISerializationConfiguration<BinaryWriter,BinaryReader>
    {
        public IEnumerable<IPrimitiveHandler<BinaryWriter, BinaryReader>> PrimitiveHandlers { get; }

        public BinaryConfiguration(IEnumerable<IPrimitiveHandler<BinaryWriter, BinaryReader>> typeHandlers = null)
        {
            PrimitiveHandlers = typeHandlers ?? new List<IPrimitiveHandler<BinaryWriter, BinaryReader>>();
        }
    }
}