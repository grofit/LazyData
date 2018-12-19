using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LazyData.Attributes;
using LazyData.Extensions;
using LazyData.Mappings.Types.Primitives;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Mappings.Types
{
    public class TypeAnalyzer : ITypeAnalyzer
    {
        public TypeAnalyzerConfiguration Configuration { get; }
        public IPrimitiveRegistry PrimitiveRegistry { get; }

        public TypeAnalyzer(IPrimitiveRegistry primitiveRegistry = null, TypeAnalyzerConfiguration configuration = null)
        {
            if (primitiveRegistry == null)
            {
                PrimitiveRegistry = new PrimitiveRegistry();
                PrimitiveRegistry.AddPrimitiveCheck(new BasicPrimitiveChecker());
            }
            else
            {
                PrimitiveRegistry = primitiveRegistry;
            }

            Configuration = configuration ?? new TypeAnalyzerConfiguration();
        }

        public bool HasImplementedGenericCollection(Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && (
                                                     x.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                                                     x.GetGenericTypeDefinition().GetInterface(nameof(IEnumerable)) !=
                                                     null));
        }
        
        public bool IsGenericCollection(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().IsAssignableFrom(typeof(ICollection<>)) ||
                       type.GetGenericTypeDefinition().GetInterface(nameof(IEnumerable)) != null);
        }

        public bool HasImplementedGenericDictionary(Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && (
                                                     x.GetGenericTypeDefinition() == typeof(IDictionary<,>) || 
                                                     x.GetGenericTypeDefinition().GetInterface(nameof(IDictionary)) != null));
        }

        public bool IsGenericDictionary(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) || 
                                        type.GetGenericTypeDefinition().GetInterface(nameof(IDictionary)) != null); 
        }

        public bool IsDynamicType(Type type)
        { return type.IsAbstract || type.IsInterface || type == typeof(object); }

        public bool IsDynamicType(PropertyInfo propertyInfo)
        {
            var typeIsDynamic = IsDynamicType(propertyInfo.PropertyType);
            return typeIsDynamic || propertyInfo.HasAttribute<DynamicTypeAttribute>();
        }

        public Type[] GetGenericTypes(Type type, Type genericType)
        {
            if (type.IsGenericType)
            { return type.GetGenericArguments(); }

            var proxyType = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);
            if (proxyType == null) { return new Type[0]; }
            return proxyType.GetGenericArguments();
        }
        
        public Type GetElementType(Type type)
        {
            if (type.IsArray) { return type.GetElementType(); }
            if (type.IsGenericType){ return type.GetGenericArguments()[0];}
            
            var proxyType = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            return proxyType.GetGenericArguments()[0];
        }

        public bool HasIgnoredTypes()
        { return Configuration.IgnoredTypes.Any(); }

        public bool IsIgnoredType(Type type)
        { return !Configuration.IgnoredTypes.Any(type.IsAssignableFrom); }

        public bool IsTypeMatch(Type actualType, Type expectedType)
        {
            if (actualType == expectedType || actualType.IsAssignableFrom(expectedType))
            { return true; }

            if (actualType.IsGenericType)
            {
                var genericType = actualType.GetGenericTypeDefinition();
                if (genericType == expectedType)
                { return true; }

                var genericInterfaces = genericType.GetInterfaces();
                if (genericInterfaces.Any(x => x.Name == expectedType.Name))
                { return true; }
            }

            var interfaces = actualType.GetInterfaces();
            return interfaces.Any(x => x == expectedType);
        }

        public Type GetNullableType(Type possibleNullable)
        { return Nullable.GetUnderlyingType(possibleNullable); }

        public bool IsPrimitiveType(Type type)
        { return PrimitiveRegistry.IsKnownPrimitive(type); }
    }
}