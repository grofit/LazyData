using System;
using System.IO;

namespace LazyData.Serialization.Binary.Handlers
{
    public class BinaryPrimitiveDeserializer
    {
        public virtual object DeserializeDefaultPrimitive(Type type, BinaryReader reader)
        {
            if (type == typeof(byte)) { return reader.ReadByte(); }
            if (type == typeof(short)) { return reader.ReadInt16(); }
            if (type == typeof(int)) { return reader.ReadInt32(); }
            if (type == typeof(long)) { return reader.ReadInt64(); }
            if (type == typeof(bool)) { return reader.ReadBoolean(); }
            if (type == typeof(float)) { return reader.ReadSingle(); }
            if (type == typeof(double)) { return reader.ReadDouble(); }
            if (type == typeof(decimal)) { return reader.ReadDecimal(); }
            if (type.IsEnum)
            {
                var value = reader.ReadInt32();
                return Enum.ToObject(type, value);
            }
            if (type == typeof(Guid))
            {
                return new Guid(reader.ReadString());
            }
            if (type == typeof(DateTime))
            {
                var binaryTime = reader.ReadInt64();
                return DateTime.FromBinary(binaryTime);
            }

            return reader.ReadString();
        }
    }
}