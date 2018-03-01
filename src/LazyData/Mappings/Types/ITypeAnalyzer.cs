using System;
using System.Reflection;

namespace LazyData.Mappings.Types
{
    public interface ITypeAnalyzer
    {
        bool IsGenericList(Type type);
        bool IsGenericDictionary(Type type);
        bool IsDynamicType(Type type);
        bool IsDynamicType(PropertyInfo propertyInfo);
        Type GetNullableType(Type type);
        bool HasIgnoredTypes();
        bool IsIgnoredType(Type type);
        bool IsPrimitiveType(Type type);
    }
}