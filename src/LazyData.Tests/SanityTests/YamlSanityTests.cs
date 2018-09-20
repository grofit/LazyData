using System;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.SuperLazy;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using LazyData.Yaml;
using Xunit;
using Xunit.Abstractions;
using Assert = LazyData.Tests.Extensions.AssertExtensions;

namespace LazyData.Tests.SanityTests
{
    public class YamlSanityTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;
        private ITestOutputHelper _testOutputHelper;

        public YamlSanityTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _typeCreator = new TypeCreator();

            var analyzer = new TypeAnalyzer();
            var mapper = new EverythingTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }
        
        [Fact]
        public void check_it_converts_sensibly()
        {
            var serializer = new YamlSerializer(_mappingRegistry);
            var deserializer = new YamlDeserializer(_mappingRegistry, _typeCreator);
            var expectedModel = SerializationTestHelper.GeneratePopulatedModel();

            var data = serializer.Serialize(expectedModel);
            _testOutputHelper.WriteLine("Outputted Yaml: ");
            _testOutputHelper.WriteLine(data.AsString);

            var actualModel = deserializer.Deserialize<ComplexModel>(data);
            SerializationTestHelper.AssertPopulatedData(expectedModel, actualModel);
        }
    }
}