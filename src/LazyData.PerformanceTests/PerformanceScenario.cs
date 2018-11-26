using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using LazyData.Binary;
using LazyData.Json;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.PerformanceTests.Models;
using LazyData.Registries;
using LazyData.Serialization;
using LazyData.Xml;

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

            _xmlSerializer = new XmlSerializer(mappingRegistry);
            _xmlDeserializer = new XmlDeserializer(mappingRegistry, typeCreator);
        }

        private DataObject SerializeModel(object model, ISerializer serializer)
        {return serializer.Serialize(model); }

        private void DeserializeModel(Type type, DataObject data, IDeserializer serializer)
        { serializer.Deserialize(type, data); }


        private Person CreatePerson()
        {
            return new Person
            {
                Age = 99999,
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
            };
        }

        private PersonList CreatePersonList()
        {
            return new PersonList
            {
                Models = Enumerable.Range(0, Iterations).Select(x => CreatePerson()).ToArray()
            };
        }

        private DynamicTypesModel CreateDynamicModel()
        {
            var model = new DynamicTypesModel();
            model.DynamicNestedProperty = CreatePerson();
            model.DynamicPrimitiveProperty = 12;

            model.DynamicList = new List<object>();
            model.DynamicList.Add(CreatePerson());
            model.DynamicList.Add("Hello");
            model.DynamicList.Add(20);

            model.DynamicDictionary = new Dictionary<object, object>();
            model.DynamicDictionary.Add("key1", 62);
            model.DynamicDictionary.Add(CreatePerson(), 54);
            model.DynamicDictionary.Add(1, CreatePerson());
            return model;            
        }
        
        [Benchmark]
        public void IterateBinarySerialization()
        {
            var model = CreatePerson();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _binarySerializer);
                DeserializeModel(typeof(Person), dataObject, _binaryDeserializer);
            }
        }

        [Benchmark]
        public void IterateJsonSerialization()
        {
            var model = CreatePerson();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _jsonSerializer);
                DeserializeModel(typeof(Person), dataObject, _jsonDeserializer);
            }
        }

        [Benchmark]
        public void IterateXmlSerialization()
        {
            var model = CreatePerson();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _xmlSerializer);
                DeserializeModel(typeof(Person), dataObject, _xmlDeserializer);
            }
        }

        [Benchmark]
        public void CollectionBinarySerialization()
        {
            var model = CreatePersonList();
            var dataObject = SerializeModel(model, _binarySerializer);
            DeserializeModel(typeof(Person), dataObject, _binaryDeserializer);
        }

        [Benchmark]
        public void CollectionJsonSerialization()
        {
            var model = CreatePersonList();
            var dataObject = SerializeModel(model, _jsonSerializer);
            DeserializeModel(typeof(Person), dataObject, _jsonDeserializer);
            
        }

        [Benchmark]
        public void CollectionXmlSerialization()
        {
            var model = CreatePersonList();
            var dataObject = SerializeModel(model, _xmlSerializer);
            DeserializeModel(typeof(Person), dataObject, _xmlDeserializer);            
        }

        [Benchmark]
        public void DynamicBinarySerialization()
        {
            var model = CreateDynamicModel();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _binarySerializer);
                DeserializeModel(typeof(Person), dataObject, _binaryDeserializer);
            }
        }

        [Benchmark]
        public void DynamicJsonSerialization()
        {
            var model = CreateDynamicModel();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _jsonSerializer);
                DeserializeModel(typeof(Person), dataObject, _jsonDeserializer);
            }
        }

        [Benchmark]
        public void DynamicXmlSerialization()
        {
            var model = CreateDynamicModel();
            for (var i = 0; i < Iterations; i++)
            {
                var dataObject = SerializeModel(model, _xmlSerializer);
                DeserializeModel(typeof(Person), dataObject, _xmlDeserializer);
            }
        }
    }
}