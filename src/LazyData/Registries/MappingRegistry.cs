using System;
using System.Collections.Generic;
using LazyData.Mappings;
using LazyData.Mappings.Mappers;

namespace LazyData.Registries
{
    public class MappingRegistry : IMappingRegistry
    {
        public ITypeMapper TypeMapper { get; }
        public IDictionary<Type, TypeMapping> TypeMappings { get; }

        public MappingRegistry(ITypeMapper typeMapper)
        {
            TypeMapper = typeMapper;
            TypeMappings = new Dictionary<Type, TypeMapping>();
        }

        public TypeMapping GetMappingFor<T>() where T : new()
        {
            var type = typeof(T);
            return GetMappingFor(type);
        }

        public TypeMapping GetMappingFor(Type type)
        {
            if(TypeMappings.ContainsKey(type))
            { return TypeMappings[type]; }

            var typeMapping = TypeMapper.GetTypeMappingsFor(type);
            TypeMappings.Add(type, typeMapping);
            return typeMapping;
        }
    }
}