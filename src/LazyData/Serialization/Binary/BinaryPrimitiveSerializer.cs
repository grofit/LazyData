using System;
using System.IO;

namespace LazyData.Serialization.Binary
{
    public class BinaryPrimitiveSerializer
    {
        public virtual void SerializeDefaultPrimitive(object value, Type type, BinaryWriter state)
        {
            if (type == typeof(byte)) { state.Write((byte)value); }
            else if (type == typeof(short)) { state.Write((short)value); }
            else if (type == typeof(int)) { state.Write((int)value); }
            else if (type == typeof(long)) { state.Write((long)value); }
            else if (type == typeof(bool)) { state.Write((bool)value); }
            else if (type == typeof(float)) { state.Write((float)value); }
            else if (type == typeof(double)) { state.Write((double)value); }
            else if (type == typeof(decimal)) { state.Write((decimal)value); }
            else if (type.IsEnum) { state.Write((int)value); }
            
            else if (type == typeof(DateTime)) { state.Write(((DateTime)value).ToBinary()); }
            else if (type == typeof(Guid)) { state.Write(((Guid)value).ToString()); }
            else if (type == typeof(string)) { state.Write(value.ToString()); }
        }
    }
}