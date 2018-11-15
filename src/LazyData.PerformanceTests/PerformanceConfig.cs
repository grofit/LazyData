using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;

namespace LazyData.PerformanceTests
{
    public class PerformanceConfig : ManualConfig
    {
        public PerformanceConfig()
        {
            Add(MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);

            var job = Job.ShortRun.WithLaunchCount(1)
                .WithIterationCount(1)
                .WithWarmupCount(1)
                .With(Runtime.Core)
                .With(Platform.X64);
            Add(job);
        }
    }
}