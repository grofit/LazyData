using System;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Mappings.Types.Primitives;
using LazyData.Mappings.Types.Primitives.Checkers;
using LazyData.Numerics.Checkers;
using LazyData.Numerics.Handlers;
using LazyData.Registries;
using LazyData.Serialization.Binary;
using LazyData.Serialization.Debug;
using LazyData.Serialization.Json;
using LazyData.Serialization.Xml;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using Xunit;
using Xunit.Abstractions;
using Assert = LazyData.Tests.Extensions.AssertExtensions;

namespace LazyData.Tests.Serialization
{
    public class NumericsModelSerializationTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;
        private ITestOutputHelper _outputHelper;

        public NumericsModelSerializationTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _typeCreator = new TypeCreator();

            var primitiveRegistry = new PrimitiveRegistry();
            primitiveRegistry.AddPrimitiveCheck(new BasicPrimitiveChecker());
            primitiveRegistry.AddPrimitiveCheck(new NumericsPrimitiveChecker());
            var analyzer = new TypeAnalyzer(primitiveRegistry);

            var mapper = new EverythingTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }

        [Fact]
        public void should_handle_numerics_data_as_binary_with_primitive_handler()
        {
            var expected = SerializationTestHelper.GenerateNumericsModel();
            var serializer = new BinarySerializer(_mappingRegistry, new []{ new NumericsBinaryPrimitiveHandler() });
            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator, new []{ new NumericsBinaryPrimitiveHandler() });

            var output = serializer.Serialize(expected);
            _outputHelper.WriteLine(output.AsString);

            var actual = deserializer.Deserialize<NumericsTypesModel>(output);

            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void should_handle_numerics_data_as_json_with_primitive_handler()
        {
            var expected = SerializationTestHelper.GenerateNumericsModel();
            var serializer = new JsonSerializer(_mappingRegistry, new[] { new NumericsJsonPrimitiveHandler() });
            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator, new[] { new NumericsJsonPrimitiveHandler() });

            var output = serializer.Serialize(expected);
            _outputHelper.WriteLine(output.AsString);

            var actual = deserializer.Deserialize<NumericsTypesModel>(output);

            Assert.AreEqual(expected, actual);
        }

        [Fact]
        public void should_handle_numerics_data_as_xml_with_primitive_handler()
        {
            var expected = SerializationTestHelper.GenerateNumericsModel();
            var serializer = new XmlSerializer(_mappingRegistry, new[] { new NumericsXmlPrimitiveHandler() });
            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator, new[] { new NumericsXmlPrimitiveHandler() });

            var output = serializer.Serialize(expected);
            _outputHelper.WriteLine(output.AsString);

            var actual = deserializer.Deserialize<NumericsTypesModel>(output);

            Assert.AreEqual(expected, actual);
        }
    }
}