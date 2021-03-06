using System;

namespace LazyData.Serialization
{
    public interface IDeserializer
    {
        object Deserialize(DataObject data, Type type = null);
        T Deserialize<T>(DataObject data) where T : new();

        void DeserializeInto(DataObject data, object existingInstance);
        void DeserializeInto<T>(DataObject data, T existingInstance);
    }
}