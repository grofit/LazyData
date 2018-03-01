using System;

namespace LazyData.Mappings.Types.Primitives.Checkers
{
    public class PredicatePrimitiveChecker : IPrimitiveChecker
    {
        private readonly Func<Type, bool> _predicate;

        public PredicatePrimitiveChecker(Func<Type, bool> predicate)
        { _predicate = predicate; }

        public bool IsPrimitive(Type type)
        { return _predicate(type); }
    }
}