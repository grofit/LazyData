using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace LazyData.PerformanceTests
{
    public class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            var config = new PerformanceConfig()
                .With(new SummaryStyle
                {
                    TimeUnit = TimeUnit.Millisecond, SizeUnit = SizeUnit.KB, PrintUnitsInHeader = true,
                    PrintUnitsInContent = true
                })
                .With(ConsoleLogger.Default)
                .With(DefaultColumnProviders.Instance);
            
            BenchmarkRunner.Run<PerformanceScenario>(config);
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
