namespace HareDu.Perf.Benchmarks
{
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;

    [Config(typeof(BenchmarkConfig))]
    public class GetSystemOverviewBenchmarks :
        HareDuPerformanceTesting
    {
        readonly ServiceProvider _services;

        public GetSystemOverviewBenchmarks()
        {
            _services = GetContainerBuilder("Benchmarks/TestData/SystemOverviewInfo.json").BuildServiceProvider();
        }

        [Benchmark]
        public async Task GetSystemOverviewBenchmark()
        {
            var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<BrokerSystem>()
                .GetSystemOverview();
        }

        [Benchmark]
        public async Task GetSystemOverviewExtensionBenchmark()
        {
            var result = await _services.GetService<IBrokerObjectFactory>()
                .GetBrokerSystemOverview();
        }
    }
}