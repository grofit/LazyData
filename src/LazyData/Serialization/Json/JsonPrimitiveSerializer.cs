using System;
using LazyData.Extensions;
using Newtonsoft.Json.Linq;

namespace LazyData.Serialization.Json
{
    public class JsonPrimitiveSerializer
    {
        private readonly Type[] CatchmentTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };
        
        public virtual void SerializeDefaultPrimitive(object value, Type type, JToken element)
        {
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)value;
                var stringValue = typedValue.ToBinary().ToString();
                element.Replace(new JValue(stringValue));
                return;
            }

            if (type.IsTypeOf(CatchmentTypes) || type.IsEnum)
            {
                element.Replace(new JValue(value));
                return;
            }
        }
    }
}