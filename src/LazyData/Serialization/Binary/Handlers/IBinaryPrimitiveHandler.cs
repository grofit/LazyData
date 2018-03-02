using System.IO;

namespace LazyData.Serialization.Binary.Handlers
{
    public interface IBinaryPrimitiveHandler : IPrimitiveHandler<BinaryWriter, BinaryReader>
    {}
}