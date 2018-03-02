using System.Collections.Generic;

namespace LazyData.Serialization
{
    public interface ISerializationConfiguration<TSerialize, TDeserialize>
    {
        IEnumerable<IPrimitiveHandler<TSerialize, TDeserialize>> PrimitiveHandlers { get; }
    }
}