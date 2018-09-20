using System;
using LazyData.Binary;
using LazyData.Json;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Serialization.Debug;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using LazyData.Xml;
using Xunit;
using Xunit.Abstractions;

namespace LazyData.Tests.Serialization
{
    public class NulledModelSerializationTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;
        private ITestOutputHelper _outputHelper;

        public NulledModelSerializationTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _typeCreator = new TypeCreator();

            var analyzer = new TypeAnalyzer();
            var mapper = new DefaultTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }

        [Fact]
        public void should_serialize_nulled_data_with_debug_serializer()
        {
            var model = SerializationTestHelper.GenerateNulledModel();
            var serializer = new DebugSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            _outputHelper.WriteLine(output.AsString);
        }
        
        [Fact]
        public void should_correctly_serialize_nulled_data_with_json()
        {
            var model = SerializationTestHelper.GenerateNulledModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            _outputHelper.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            _outputHelper.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Fact]
        public void should_correctly_serialize_nulled_data_into_existing_object_with_json()
        {
            var model = SerializationTestHelper.GenerateNulledModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            _outputHelper.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            _outputHelper.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = new ComplexModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Fact]
        public void should_correctly_serialize_nulled_data_with_binary()
        {
            var model = SerializationTestHelper.GenerateNulledModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            _outputHelper.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            _outputHelper.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Fact]
        public void should_correctly_serialize_nulled_data_into_existing_object_with_binary()
        {
            var model = SerializationTestHelper.GenerateNulledModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            _outputHelper.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            _outputHelper.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = new ComplexModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Fact]
        public void should_correctly_serialize_nulled_data_with_xml()
        {
            var model = SerializationTestHelper.GenerateNulledModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            _outputHelper.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            _outputHelper.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Fact]
        public void should_correctly_serialize_nulled_data_into_existing_object_with_xml()
        {
            var model = SerializationTestHelper.GenerateNulledModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            _outputHelper.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            _outputHelper.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = new ComplexModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertNulledData(model, result);
        }
    }
}