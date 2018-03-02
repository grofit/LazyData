using System;
using System.Numerics;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Numerics.Checkers;
using LazyData.Serialization.Json.Handlers;
using Newtonsoft.Json.Linq;

namespace LazyData.Numerics.Handlers
{
    public class NumericsJsonPrimitiveHandler : IJsonPrimitiveHandler
    {
        public IPrimitiveChecker PrimitiveChecker { get; } = new NumericsPrimitiveChecker();

        public void Serialize(JToken state, object data, Type type)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)data;
                state["x"] = typedObject.X;
                state["y"] = typedObject.Y;
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)data;
                state["x"] = typedObject.X;
                state["y"] = typedObject.Y;
                state["z"] = typedObject.Z;
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)data;
                state["x"] = typedObject.X;
                state["y"] = typedObject.Y;
                state["z"] = typedObject.Z;
                state["w"] = typedObject.W;
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)data;
                state["x"] = typedObject.X;
                state["y"] = typedObject.Y;
                state["z"] = typedObject.Z;
                state["w"] = typedObject.W;
                return;
            }
        }

        public object Deserialize(JToken state, Type type)
        {
            if (type == typeof(Vector2))
            { return new Vector2(state["x"].ToObject<float>(), state["y"].ToObject<float>()); }
            if (type == typeof(Vector3))
            { return new Vector3(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>()); }
            if (type == typeof(Vector4))
            { return new Vector4(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>(), state["w"].ToObject<float>()); }
            if (type == typeof(Quaternion))
            { return new Quaternion(state["x"].ToObject<float>(), state["y"].ToObject<float>(), state["z"].ToObject<float>(), state["w"].ToObject<float>()); }

            return null;
        }
    }
}