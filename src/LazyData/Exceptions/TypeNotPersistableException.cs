using System;

namespace LazyData.Exceptions
{
    public class TypeNotPersistableException : Exception
    {
        public TypeNotPersistableException(Type type) : base($"{type} is not persistable, ensure it has a Persist attribute")
        {}
    }
}