using System;
using System.Xml.Linq;
using LazyData.Extensions;

namespace LazyData.Serialization.Xml.Handlers
{
    public class XmlPrimitiveSerializer
    {
        private readonly Type[] CatchmentTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };

        public virtual void SerializeDefaultPrimitive(object value, Type type, XElement element)
        {
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Value = stringValue;
                return;
            }

            if (type.IsTypeOf(CatchmentTypes) || type.IsEnum)
            {
                element.Value = value.ToString();
                return;
            }
        }
    }
}