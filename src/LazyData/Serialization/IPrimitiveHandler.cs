using System;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Serialization
{
    public interface IPrimitiveHandler<Tin, Tout>
    {
        IPrimitiveChecker PrimitiveChecker { get; }
        void Serialize(Tin state, object data, Type type);
        object Deserialize(Tout state, Type type);
    }
}