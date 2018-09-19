using System.IO;
using LazyData.Serialization;

namespace LazyData.Binary.Handlers
{
    public interface IBinaryPrimitiveHandler : IPrimitiveHandler<BinaryWriter, BinaryReader>
    {}
}