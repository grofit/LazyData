using System;
using System.Xml.Linq;
using LazyData.Extensions;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Serialization.Xml.Handlers
{
    public class BasicXmlPrimitiveHandler : IXmlPrimitiveHandler
    {
        private readonly Type[] StringCompatibleTypes =
        {
            typeof(string), typeof(bool), typeof(byte), typeof(short), typeof(int),
            typeof(long), typeof(Guid), typeof(float), typeof(double), typeof(decimal)
        };

        public IPrimitiveChecker PrimitiveChecker { get; } = new BasicPrimitiveChecker();

        public void Serialize(XElement state, object data, Type type)
        {
            if (type == typeof(DateTime))
            {
                var typedValue = (DateTime)data;
                var stringValue = typedValue.ToBinary().ToString();
                state.Value = stringValue;
                return;
            }

            if (type == typeof(TimeSpan))
            {
                var typedValue = (TimeSpan)data;
                var stringValue = typedValue.TotalMilliseconds.ToString();
                state.Value = stringValue;
                return;
            }

            if (type.IsTypeOf(StringCompatibleTypes) || type.IsEnum)
            {
                state.Value = data.ToString();
                return;
            }
        }

        public object Deserialize(XElement state, Type type)
        {
            if (type == typeof(byte)) { return byte.Parse(state.Value); }
            if (type == typeof(short)) { return short.Parse(state.Value); }
            if (type == typeof(int)) { return int.Parse(state.Value); }
            if (type == typeof(long)) { return long.Parse(state.Value); }
            if (type == typeof(bool)) { return bool.Parse(state.Value); }
            if (type == typeof(float)) { return float.Parse(state.Value); }
            if (type == typeof(double)) { return double.Parse(state.Value); }
            if (type == typeof(decimal)) { return decimal.Parse(state.Value); }
            if (type.IsEnum) { return Enum.Parse(type, state.Value); }

            if (type == typeof(Guid))
            {
                return new Guid(state.Value);
            }

            if (type == typeof(DateTime))
            {
                var binaryTime = long.Parse(state.Value);
                return DateTime.FromBinary(binaryTime);
            }

            if (type == typeof(TimeSpan))
            {
                var milliseconds = double.Parse(state.Value);
                return TimeSpan.FromMilliseconds(milliseconds);
            }

            return state.Value;
        }
    }
}