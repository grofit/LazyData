using System;
using System.Numerics;
using System.Xml.Linq;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Numerics.Checkers;
using LazyData.Serialization.Xml.Handlers;

namespace LazyData.Numerics.Handlers
{
    public class NumericsXmlPrimitiveHandler : IXmlPrimitiveHandler
    {
        public IPrimitiveChecker PrimitiveChecker { get; } = new NumericsPrimitiveChecker();

        public void Serialize(XElement state, object data, Type type)
        {
            if (type == typeof(Vector2))
            {
                var typedObject = (Vector2)data;
                state.Add(new XElement("x", typedObject.X));
                state.Add(new XElement("y", typedObject.Y));
                return;
            }
            if (type == typeof(Vector3))
            {
                var typedObject = (Vector3)data;
                state.Add(new XElement("x", typedObject.X));
                state.Add(new XElement("y", typedObject.Y));
                state.Add(new XElement("z", typedObject.Z));
                return;
            }
            if (type == typeof(Vector4))
            {
                var typedObject = (Vector4)data;
                state.Add(new XElement("x", typedObject.X));
                state.Add(new XElement("y", typedObject.Y));
                state.Add(new XElement("z", typedObject.Z));
                state.Add(new XElement("w", typedObject.W));
                return;
            }
            if (type == typeof(Quaternion))
            {
                var typedObject = (Quaternion)data;
                state.Add(new XElement("x", typedObject.X));
                state.Add(new XElement("y", typedObject.Y));
                state.Add(new XElement("z", typedObject.Z));
                state.Add(new XElement("w", typedObject.W));
                return;
            }
        }

        public object Deserialize(XElement state, Type type)
        {
            if (type == typeof(Vector2))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                var z = float.Parse(state.Element("z").Value);
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                var z = float.Parse(state.Element("z").Value);
                var w = float.Parse(state.Element("w").Value);
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Quaternion))
            {
                var x = float.Parse(state.Element("x").Value);
                var y = float.Parse(state.Element("y").Value);
                var z = float.Parse(state.Element("z").Value);
                var w = float.Parse(state.Element("w").Value);
                return new Quaternion(x, y, z, w);
            }

            return null;
        }
    }
}