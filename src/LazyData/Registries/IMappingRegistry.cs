using System;
using LazyData.Mappings;
using LazyData.Mappings.Mappers;

namespace LazyData.Registries
{
    public interface IMappingRegistry
    {
        ITypeMapper TypeMapper { get; }

        TypeMapping GetMappingFor<T>() where T : new();
        TypeMapping GetMappingFor(Type type);
    }
}