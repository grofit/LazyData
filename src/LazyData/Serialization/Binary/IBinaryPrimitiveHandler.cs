using System.IO;

namespace LazyData.Serialization.Binary
{
    public interface IBinaryPrimitiveHandler : IPrimitiveHandler<BinaryWriter, BinaryReader>
    {}
}