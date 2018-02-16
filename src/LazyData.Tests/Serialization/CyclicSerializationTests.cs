using System;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Serialization.Json;
using LazyData.Tests.Models;
using Xunit;
using Xunit.Abstractions;

namespace LazyData.Tests.Serialization
{
    public class CyclicSerializationTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;
        private ITestOutputHelper _testOutputHelper;

        public CyclicSerializationTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            
            _typeCreator = new TypeCreator();

            var analyzer = new TypeAnalyzer();
            var mapper = new EverythingTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }

        //[Fact]
        public void should_throw_exception_with_cyclic_dependency()
        {
            var a = new CyclicA();
            var b = new CyclicB();

            a.References = b;
            b.References = a;

            var serializer = new JsonSerializer(_mappingRegistry);
            
            Assert.Throws<Exception>(() =>
            {
                var output = serializer.Serialize(a);
                _testOutputHelper.WriteLine(output.AsString);
            });
        }
    }
}