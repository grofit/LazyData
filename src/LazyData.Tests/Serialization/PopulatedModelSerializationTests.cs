using System;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Serialization.Binary;
using LazyData.Serialization.Debug;
using LazyData.Serialization.Json;
using LazyData.Serialization.Xml;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using NUnit.Framework;

namespace LazyData.Tests.Serialization
{
    [TestFixture]
    public class PopulatedModelSerializationTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;

        [SetUp]
        public void Setup()
        {
            _typeCreator = new TypeCreator();

            var analyzer = new TypeAnalyzer();
            var mapper = new DefaultTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }
        
        [Test]
        public void should_serialize_populated_data_with_debug_serializer()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();
            var serializer = new DebugSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine(output.AsString);
        }
        
        [Test]
        public void should_correctly_serialize_populated_data_with_json()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_into_existing_object_with_json()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = new ComplexModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_into_existing_object_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = new ComplexModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_into_existing_object_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = new ComplexModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }
    }
}