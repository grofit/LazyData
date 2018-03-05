using System;
using BenchmarkDotNet.Running;

namespace LazyData.PerformanceTests
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<PerformanceScenario>();
        }
    }
}
