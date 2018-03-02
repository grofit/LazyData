using System.Numerics;

namespace LazyData.Tests.Models
{
    public class NumericsTypesModel
    {
        public Vector2 Vector2Value { get; set; }
        public Vector3 Vector3Value { get; set; }
        public Vector4 Vector4Value { get; set; }
        public Quaternion QuaternionValue { get; set; }

        public Vector2? NullableVector2Value { get; set; }
        public Vector3? NullableVector3Value { get; set; }
        public Vector4? NullableVector4Value { get; set; }
        public Quaternion? NullableQuaternionValue { get; set; }
    }
}