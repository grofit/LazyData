using System;
using System.Collections.Generic;
using LazyData.Bson;
using LazyData.Json;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.SuperLazy;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using Xunit;
using Xunit.Abstractions;
using Assert = LazyData.Tests.Extensions.AssertExtensions;

namespace LazyData.Tests.SanityTests
{   
    public class ContainsDictionary
    {
        public string Something { get; set; }
        public CustomDictionary Proxy { get; set; } = new CustomDictionary();
    }
    
    public class GeneralTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;
        private ITestOutputHelper _testOutputHelper;

        public GeneralTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _typeCreator = new TypeCreator();

            var analyzer = new TypeAnalyzer();
            var mapper = new EverythingTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }
        
        [Fact]
        public void check_json_can_convert_object_with_dictionary()
        {
            var serializer = new JsonSerializer(_mappingRegistry);
            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);

            var obj = new ContainsDictionary {Something = "blah"};
            obj.Proxy.Add(1, 2);
            obj.Proxy.Add(3, 4);
            
            var data = serializer.Serialize(obj);
            _testOutputHelper.WriteLine("Outputted Json: ");
            _testOutputHelper.WriteLine(data.AsString);

            var actualModel = deserializer.Deserialize<ContainsDictionary>(data);
            Assert.NotNull(actualModel);
        }
    }
}