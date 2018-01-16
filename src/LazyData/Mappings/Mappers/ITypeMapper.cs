using System;
using LazyData.Mappings.Types;

namespace LazyData.Mappings.Mappers
{
    public interface ITypeMapper
    {
        ITypeAnalyzer TypeAnalyzer { get; }
        TypeMapping GetTypeMappingsFor(Type type);       
    }
}