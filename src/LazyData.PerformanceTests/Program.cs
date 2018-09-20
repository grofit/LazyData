using BenchmarkDotNet.Running;

namespace LazyData.PerformanceTests
{
    public class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            BenchmarkRunner.Run<PerformanceScenario>();
#elif DEBUG          
            var scenario = new PerformanceScenario();
            scenario.Iterations = 10;
            scenario.Setup();
            
            scenario.CollectionBinarySerialization();
            scenario.CollectionJsonSerialization();
            scenario.CollectionXmlSerialization();
            scenario.DynamicBinarySerialization();
            scenario.DynamicJsonSerialization();
            scenario.DynamicXmlSerialization();
            scenario.IterateBinarySerialization();
            scenario.IterateJsonSerialization();
            scenario.IterateXmlSerialization();
#endif
        }
    }
}
