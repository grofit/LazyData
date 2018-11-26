using System;
using System.Collections.Generic;
using LazyData.Bson;
using LazyData.Json;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using Xunit;
using Xunit.Abstractions;

namespace LazyData.Tests.SanityTests
{
    public class ModelA
    {
        public int Id { get; set; }
        public IEnumerable<C> Data { get; set; }
    }

    public class ModelB
    {
        public int Id { get; set; }
        public List<C> Data { get; set; }
    }
    
    public class ModelInterchangableTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;
        private ITestOutputHelper _testOutputHelper;

        public ModelInterchangableTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _typeCreator = new TypeCreator();

            var analyzer = new TypeAnalyzer();
            var mapper = new EverythingTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }
        
        [Fact]
        public void check_it_can_convert_from_a_to_b_to_a()
        {
            var serializer = new JsonSerializer(_mappingRegistry);
            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);

            var child1 = new C {FloatValue = 22};
            var child2 = new C {FloatValue = 30};
            var child3 = new C {FloatValue = 1};
            
            var startingModel = new ModelA
            {
                Id = 1,
                Data = new[] { child1, child2 }
            };

            var data = serializer.Serialize(startingModel);
            _testOutputHelper.WriteLine("Starting JSON: ");
            _testOutputHelper.WriteLine(data.AsString);

            var interimModel = deserializer.Deserialize<ModelB>(data);
            interimModel.Data.Add(child3);

            var interimData = serializer.Serialize(interimModel);
            _testOutputHelper.WriteLine("Interim JSON: ");
            _testOutputHelper.WriteLine(interimData.AsString);
            
            var actualModel = deserializer.Deserialize<ModelA>(interimData);
            
            Assert.Equal(startingModel.Id, actualModel.Id);
            Assert.NotNull(actualModel.Data);
            Assert.Contains(actualModel.Data, x => x.FloatValue == child1.FloatValue);
            Assert.Contains(actualModel.Data, x => x.FloatValue == child2.FloatValue);
            Assert.Contains(actualModel.Data, x => x.FloatValue == child3.FloatValue);
        }
    }
}