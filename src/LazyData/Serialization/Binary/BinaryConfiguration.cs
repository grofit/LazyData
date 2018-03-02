using System.IO;

namespace LazyData.Serialization.Binary
{
    public class BinaryConfiguration : SerializationConfiguration<BinaryWriter,BinaryReader>
    {
        public static BinaryConfiguration Default => new BinaryConfiguration
        {
            TypeHandlers = new IPrimitiveHandler<BinaryWriter, BinaryReader>[0]
        };
    }
}