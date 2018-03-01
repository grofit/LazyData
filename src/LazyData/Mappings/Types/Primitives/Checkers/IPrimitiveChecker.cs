using System;

namespace LazyData.Mappings.Types.Primitives.Checkers
{
    public interface IPrimitiveChecker
    {
        bool IsPrimitive(Type type);
    }
}