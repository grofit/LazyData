using System;
using LazyData.SuperLazy;
using LazyData.Tests.Helpers;
using LazyData.Tests.Models;
using Xunit;
using Xunit.Abstractions;
using Assert = LazyData.Tests.Extensions.AssertExtensions;

namespace LazyData.Tests.SanityTests
{
    public class SuperLazyTransformerTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public SuperLazyTransformerTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }
        
        [Fact]
        public void check_it_converts_sensibly()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();
            var expectdString = Transformer.ToJson(model);

            testOutputHelper.WriteLine("Outputted Json: ");
            testOutputHelper.WriteLine(expectdString);
            var xml = Transformer.FromJsonToXml<ComplexModel>(expectdString);
            
            testOutputHelper.WriteLine("Converted Xml: ");
            testOutputHelper.WriteLine(xml);
            var binary = Transformer.FromXmlToBinary<ComplexModel>(xml);
            
            testOutputHelper.WriteLine("Converted Binary: ");
            testOutputHelper.WriteLine(BitConverter.ToString(binary));
            var json = Transformer.FromBinaryToJson<ComplexModel>(binary);
            
            testOutputHelper.WriteLine("Outputted Json: ");
            testOutputHelper.WriteLine(json);
            Assert.AreEqual(expectdString, json);
        }
    }
}