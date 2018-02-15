using LazyData.Extensions;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using Xunit;

namespace LazyData.Tests.TypeMapper
{
    public class DefaultTypeMapperTests
    {
        [Fact]
        public void should_correctly_map_complex_type()
        {
            var type = typeof(ComplexModel);
            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);
            var typeMapping = typeMapper.GetTypeMappingsFor(type);

            Assert.NotNull(typeMapping);
            Assert.Equal(type, typeMapping.Type);
            Assert.Equal(type.GetPersistableName(), typeMapping.Name);

            TypeAssertionHelper.AssertComplexModel(typeMapping.InternalMappings);
        }

        [Fact]
        public void should_correctly_map_dynamic_type()
        {
            var type = typeof(DynamicTypesModel);
            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);
            var typeMapping = typeMapper.GetTypeMappingsFor(type);

            Assert.NotNull(typeMapping);
            Assert.Equal(type, typeMapping.Type);
            Assert.Equal(type.GetPersistableName(), typeMapping.Name);

            TypeAssertionHelper.AssertDynamicModel(typeMapping.InternalMappings);
        }
    }
}