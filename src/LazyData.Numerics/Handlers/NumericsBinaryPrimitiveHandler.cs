using System;
using System.IO;
using System.Numerics;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Numerics.Checkers;
using LazyData.Serialization.Binary.Handlers;

namespace LazyData.Numerics.Handlers
{
    public class NumericsBinaryPrimitiveHandler : IBinaryPrimitiveHandler
    {
        public IPrimitiveChecker PrimitiveChecker { get; } = new NumericsPrimitiveChecker();

        public void Serialize(BinaryWriter state, object data, Type type)
        {
            if (type == typeof(Vector2))
            {
                var vector = (Vector2)data;
                state.Write(vector.X);
                state.Write(vector.Y);
            }
            else if (type == typeof(Vector3))
            {
                var vector = (Vector3)data;
                state.Write(vector.X);
                state.Write(vector.Y);
                state.Write(vector.Z);
            }
            else if (type == typeof(Vector4))
            {
                var vector = (Vector4)data;
                state.Write(vector.X);
                state.Write(vector.Y);
                state.Write(vector.Z);
                state.Write(vector.W);
            }
            else if (type == typeof(Quaternion))
            {
                var quaternion = (Quaternion)data;
                state.Write(quaternion.X);
                state.Write(quaternion.Y);
                state.Write(quaternion.Z);
                state.Write(quaternion.W);
            }
        }

        public object Deserialize(BinaryReader state, Type type)
        {
            if (type == typeof(Vector2))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                var z = state.ReadSingle();
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                var z = state.ReadSingle();
                var w = state.ReadSingle();
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Quaternion))
            {
                var x = state.ReadSingle();
                var y = state.ReadSingle();
                var z = state.ReadSingle();
                var w = state.ReadSingle();
                return new Quaternion(x, y, z, w);
            }

            return null;
        }
    }
}