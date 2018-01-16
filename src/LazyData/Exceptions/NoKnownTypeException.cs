using System;

namespace LazyData.Exceptions
{
    public class NoKnownTypeException : Exception
    {
        public NoKnownTypeException(Type type) : base($"{type} is not a known type")
        {}
    }
}