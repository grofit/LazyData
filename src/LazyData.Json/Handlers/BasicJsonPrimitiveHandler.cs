using System;
using LazyData.Extensions;
using LazyData.Mappings.Types.Primitives.Checkers;
using Newtonsoft.Json.Linq;

namespace LazyData.Json.Handlers
{
    public class BasicJsonPrimitiveHandler : IJsonPrimitiveHandler
    {
        private readonly Type[] StringCompatibleTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };

        public IPrimitiveChecker PrimitiveChecker { get; } = new BasicPrimitiveChecker();

        public void Serialize(JToken state, object data, Type type)
        {
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)data;
                var stringValue = typedValue.ToBinary().ToString();
                state.Replace(new JValue(stringValue));
                return;
            }

            if (type == typeof(TimeSpan))
            {
                var typedValue = (TimeSpan)data;
                var stringValue = typedValue.TotalMilliseconds.ToString();
                state.Replace(new JValue(stringValue));
                return;
            }

            if (type.IsTypeOf(StringCompatibleTypes) || type.IsEnum)
            {
                state.Replace(new JValue(data));
            }
        }

        public object Deserialize(JToken state, Type type)
        {
            if (type == typeof(DateTime))
            {
                var binaryDate = state.ToObject<long>();
                return DateTime.FromBinary(binaryDate);
            }

            if (type == typeof(TimeSpan))
            {
                var binaryDate = state.ToObject<double>();
                return TimeSpan.FromMilliseconds(binaryDate);
            }

            if (type.IsEnum)
            { return Enum.Parse(type, state.ToString()); }

            return state.ToObject(type);
        }
    }
}