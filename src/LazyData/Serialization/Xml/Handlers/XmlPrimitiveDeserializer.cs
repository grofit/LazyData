using System;
using System.Xml.Linq;

namespace LazyData.Serialization.Xml.Handlers
{
    public class XmlPrimitiveDeserializer
    {
        public virtual object DeserializeDefaultPrimitive(Type type, XElement element)
        {
            if (type == typeof(byte)) { return byte.Parse(element.Value); }
            if (type == typeof(short)) { return short.Parse(element.Value); }
            if (type == typeof(int)) { return int.Parse(element.Value); }
            if (type == typeof(long)) { return long.Parse(element.Value); }
            if (type == typeof(bool)) { return bool.Parse(element.Value); }
            if (type == typeof(float)) { return float.Parse(element.Value); }
            if (type == typeof(double)) { return double.Parse(element.Value); }
            if (type == typeof(decimal)) { return decimal.Parse(element.Value); }
            if (type.IsEnum) { return Enum.Parse(type, element.Value); }
            
            if (type == typeof(Guid))
            {
                return new Guid(element.Value);
            }
            if (type == typeof(DateTime))
            {
                var binaryTime = long.Parse(element.Value);
                return DateTime.FromBinary(binaryTime);
            }

            return element.Value;
        }
    }
}