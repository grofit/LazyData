using System;
using System.Numerics;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Numerics.Checkers
{
    public class NumericsPrimitiveChecker : IPrimitiveChecker
    {
        public bool IsPrimitive(Type type)
        {
            return type == typeof(Vector2) ||
                   type == typeof(Vector3) ||
                   type == typeof(Vector4) ||
                   type == typeof(Quaternion);
        }
    }
}