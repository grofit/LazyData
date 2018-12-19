using System;
using System.Reflection;

namespace LazyData.Mappings.Types
{
    public interface ITypeAnalyzer
    {
        bool IsGenericCollection(Type type);
        bool IsGenericDictionary(Type type);
        bool IsDynamicType(Type type);
        bool IsDynamicType(PropertyInfo propertyInfo);
        Type GetNullableType(Type type);
        Type[] GetGenericTypes(Type type, Type matching);
        Type GetElementType(Type type);
        bool HasIgnoredTypes();
        bool IsIgnoredType(Type type);
        bool IsPrimitiveType(Type type);
        bool HasImplementedGenericCollection(Type type);
        bool HasImplementedGenericDictionary(Type type);
    }
}