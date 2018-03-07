using System.Collections.Generic;
using System.Xml.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.PerformanceTests.Models;
using LazyData.Registries;
using LazyData.Serialization;
using LazyData.Serialization.Binary;
using LazyData.Serialization.Json;
using LazyData.Serialization.Xml;

namespace LazyData.PerformanceTests
{
    [Config(typeof(PerformanceConfig))]
    public class PerformanceScenario
    {
        [Params(1, 1000)]
        public int Iterations;

        private IBinarySerializer _binarySerializer;
        private IBinaryDeserializer _binaryDeserializer;

        private IJsonSerializer _jsonSerializer;
        private IJsonDeserializer _jsonDeserializer;

        private IXmlSerializer _xmlSerializer;
        private IXmlDeserializer _xmlDeserializer;

        [GlobalSetup]
        public void Setup()
        {
            var typeCreator = new TypeCreator();
            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);

            _binarySerializer = new BinarySerializer(mappingRegistry);
            _binaryDeserializer = new BinaryDeserializer(mappingRegistry, typeCreator);

            _jsonSerializer = new JsonSerializer(mappingRegistry);
            _jsonDeserializer = new JsonDeserializer(mappingRegistry, typeCreator);

            _xmlSerializer = new Serialization.Xml.XmlSerializer(mappingRegistry);
            _xmlDeserializer = new XmlDeserializer(mappingRegistry, typeCreator);
        }

        private DataObject SerializeModel(object model, ISerializer serializer)
        {return serializer.Serialize(model); }

        private void DeserializeModel(DataObject data, IDeserializer serializer)
        { serializer.Deserialize(data); }


        private Person CreatePerson()
        {
            return new Person
            {
                Age = 99999,
                FirstName = "John",
                LastName = "Doe",
                Sex = Sex.Male,
            };
        }
        
        [Benchmark]
        public void IterateBinarySerialization()
        {
            var model = CreatePerson();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _binarySerializer);
                DeserializeModel(dataObject, _binaryDeserializer);
            }
        }

        [Benchmark]
        public void IterateJsonSerialization()
        {
            var model = CreatePerson();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _jsonSerializer);
                DeserializeModel(dataObject, _jsonDeserializer);
            }
        }

        [Benchmark]
        public void IterateXmlSerialization()
        {
            var model = CreatePerson();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _xmlSerializer);
                DeserializeModel(dataObject, _xmlDeserializer);
            }
        }
    }
}