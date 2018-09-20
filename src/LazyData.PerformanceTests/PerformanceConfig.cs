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

            this.With(new SummaryStyle {TimeUnit = TimeUnit.Millisecond, SizeUnit = SizeUnit.KB, PrintUnitsInHeader = true, PrintUnitsInContent = true});
            
            var baseConfig = Job.ShortRun.WithLaunchCount(1).WithTargetCount(1).WithWarmupCount(1);
            Add(baseConfig.With(Runtime.Core).With(Platform.X64));
        }
    }
}