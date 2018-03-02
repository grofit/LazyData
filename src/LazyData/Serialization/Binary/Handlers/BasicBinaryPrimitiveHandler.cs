using System;
using System.IO;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Serialization.Binary.Handlers
{
    public class BasicBinaryPrimitiveHandler : IBinaryPrimitiveHandler
    {
        public IPrimitiveChecker PrimitiveChecker { get; } = new BasicPrimitiveChecker();

        public void Serialize(BinaryWriter state, object data, Type type)
        {
            if (type == typeof(byte)) { state.Write((byte)data); }
            else if (type == typeof(short)) { state.Write((short)data); }
            else if (type == typeof(int)) { state.Write((int)data); }
            else if (type == typeof(long)) { state.Write((long)data); }
            else if (type == typeof(bool)) { state.Write((bool)data); }
            else if (type == typeof(float)) { state.Write((float)data); }
            else if (type == typeof(double)) { state.Write((double)data); }
            else if (type == typeof(decimal)) { state.Write((decimal)data); }
            else if (type.IsEnum) { state.Write((int)data); }
            else if (type == typeof(TimeSpan)) { state.Write(((TimeSpan)data).TotalMilliseconds); }
            else if (type == typeof(DateTime)) { state.Write(((DateTime)data).ToBinary()); }
            else if (type == typeof(Guid)) { state.Write(((Guid)data).ToString()); }
            else if (type == typeof(string)) { state.Write(data.ToString()); }
        }

        public object Deserialize(BinaryReader state, Type type)
        {
            if (type == typeof(byte)) { return state.ReadByte(); }
            if (type == typeof(short)) { return state.ReadInt16(); }
            if (type == typeof(int)) { return state.ReadInt32(); }
            if (type == typeof(long)) { return state.ReadInt64(); }
            if (type == typeof(bool)) { return state.ReadBoolean(); }
            if (type == typeof(float)) { return state.ReadSingle(); }
            if (type == typeof(double)) { return state.ReadDouble(); }
            if (type == typeof(decimal)) { return state.ReadDecimal(); }
            if (type.IsEnum)
            {
                var value = state.ReadInt32();
                return Enum.ToObject(type, value);
            }
            if (type == typeof(Guid))
            {
                return new Guid(state.ReadString());
            }
            if (type == typeof(DateTime))
            {
                var binaryTime = state.ReadInt64();
                return DateTime.FromBinary(binaryTime);
            }
            if (type == typeof(TimeSpan))
            {
                var totalMilliseconds = state.ReadDouble();
                return TimeSpan.FromMilliseconds(totalMilliseconds);
            }

            return state.ReadString();
        }
    }
}