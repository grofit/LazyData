using System;

namespace LazyData.Mappings.Types.Primitives.Checkers
{
    public class BasicPrimitiveChecker : IPrimitiveChecker
    {
        public bool IsPrimitive(Type type)
        {
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid) ||
                   type.IsEnum;
        }
    }
}