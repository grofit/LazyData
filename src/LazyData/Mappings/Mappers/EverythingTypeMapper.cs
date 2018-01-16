using LazyData.Mappings.Types;

namespace LazyData.Mappings.Mappers
{
    /// <summary>
    /// This type mapper doesnt care about attributes
    /// and will attempt to map everything and anything
    /// in a class, so use with caution
    /// </summary>
    public class EverythingTypeMapper : TypeMapper
    {
        public EverythingTypeMapper(ITypeAnalyzer typeAnalyzer, MappingConfiguration configuration = null) : base(typeAnalyzer, configuration)
        {}
    }
}