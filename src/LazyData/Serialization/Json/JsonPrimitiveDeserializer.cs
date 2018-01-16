using System;
using Newtonsoft.Json.Linq;

namespace LazyData.Serialization.Json
{
    public class JsonPrimitiveDeserializer
    {
        public object DeserializeDefaultPrimitive(Type type, JToken state)
        {
            if (type == typeof(DateTime))
            {
                var binaryDate = state.ToObject<long>();
                return DateTime.FromBinary(binaryDate);
            }
            if (type.IsEnum) { return Enum.Parse(type, state.ToString()); }

            return state.ToObject(type);
        }
    }
}