using System;
using System.Linq;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Serialization;
using LazyData.Serialization.Binary;
using LazyData.Serialization.Json;
using LazyData.Serialization.Xml;
using LazyData.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace LazyData.Tests.PerformanceTest
{
    public class PerformanceScenarios
    {
        const int Iterations = 10000;
        private ITestOutputHelper testOutputHelper;

        public PerformanceScenarios(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        private void RunSerializeAndDeserializeStep(object model, object modelList, ISerializer serializer, IDeserializer deserializer)
        {
            var startTime = DateTime.Now;
            for (var i = 0; i < Iterations; i++)
            { serializer.Serialize(model); }
            var endTime = DateTime.Now;
            var totalTime = endTime - startTime;
            var average = totalTime.TotalMilliseconds / Iterations;
            testOutputHelper.WriteLine("Serialized {0} Entities in {1} with {2}ms average", Iterations, totalTime, average);

            startTime = DateTime.Now;
            var output = serializer.Serialize(modelList);
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            testOutputHelper.WriteLine("Serialized Large Entity with {0} elements in {1}", Iterations, totalTime);
            testOutputHelper.WriteLine("Large Entity Size {0}bytes", output.AsBytes.Length);

            startTime = DateTime.Now;
            deserializer.Deserialize(output);
            endTime = DateTime.Now;
            totalTime = endTime - startTime;
            testOutputHelper.WriteLine("Deserialized Large Entity with {0} elements in {1}", Iterations, totalTime);
        }

        private void RunStepsForFormats(MappingRegistry mappingRegistry, object model, object modelList)
        {
            var typeCreator = new TypeCreator();

            ISerializer serializer;
            IDeserializer deserializer;
            DataObject warmupOutput;

            // Binary Warmup
            serializer = new BinarySerializer(mappingRegistry);
            serializer.Serialize(model);
            warmupOutput = serializer.Serialize(modelList);
            deserializer = new BinaryDeserializer(mappingRegistry, typeCreator);
            deserializer.Deserialize(warmupOutput);

            testOutputHelper.WriteLine("");
            testOutputHelper.WriteLine("Binary Serializing");
            RunSerializeAndDeserializeStep(model, modelList, serializer, deserializer);
            testOutputHelper.WriteLine("");

            // JSON Warmup
            serializer = new JsonSerializer(mappingRegistry);
            serializer.Serialize(model);
            warmupOutput = serializer.Serialize(modelList);
            deserializer = new JsonDeserializer(mappingRegistry, typeCreator);
            deserializer.Deserialize(warmupOutput);

            testOutputHelper.WriteLine("");
            testOutputHelper.WriteLine("Json Serializing");
            RunSerializeAndDeserializeStep(model, modelList, serializer, deserializer);
            testOutputHelper.WriteLine("");

            // XML Warmup
            serializer = new XmlSerializer(mappingRegistry);
            serializer.Serialize(model);
            warmupOutput = serializer.Serialize(modelList);
            deserializer = new XmlDeserializer(mappingRegistry, typeCreator);
            deserializer.Deserialize(warmupOutput);

            testOutputHelper.WriteLine("");
            testOutputHelper.WriteLine("Xml Serializing");
            RunSerializeAndDeserializeStep(model, modelList, serializer, deserializer);
            testOutputHelper.WriteLine("");
        }

        [Fact]
        public void test_performance_with_simple_models()
        {
            var model = new Person
            {
                Age = 99999,
                FirstName = "Windows",
                LastName = "Server",
                Sex = Sex.Male,
            };

            var modelList = new PersonList();
            modelList.Models = Enumerable.Range(Iterations, Iterations)
                .Select(x => new Person { Age = x, FirstName = "Windows", LastName = "Server", Sex = Sex.Female })
                .ToArray();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);

            RunStepsForFormats(mappingRegistry, model, modelList);
        }

        [Fact]
        public void test_performance_with_complex_models()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var modelList = new ComplexModelList();
            modelList.Models = Enumerable.Range(Iterations, Iterations)
                .Select(x => SerializationTestHelper.GeneratePopulatedModel())
                .ToArray();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);

            RunStepsForFormats(mappingRegistry, model, modelList);
        }

        [Fact]
        public void test_performance_with_dynamic_models()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();

            var modelList = new DynamicModelList();
            modelList.Models = Enumerable.Range(Iterations, Iterations)
                .Select(x => SerializationTestHelper.GeneratePopulatedDynamicTypesModel())
                .ToArray();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new EverythingTypeMapper(typeAnalyzer);
            var mappingRegistry = new MappingRegistry(typeMapper);

            RunStepsForFormats(mappingRegistry, model, modelList);
        }
    }
}