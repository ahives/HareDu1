namespace HareDu.Perf.Benchmarks
{
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;

    [Config(typeof(BenchmarkConfig))]
    public class GetAllChannelBenchmarks :
        HareDuPerformanceTesting
    {
        readonly ServiceProvider _services;

        public GetAllChannelBenchmarks()
        {
            _services = GetContainerBuilder("Benchmarks/TestData/ChannelInfo.json").BuildServiceProvider();
        }

        [Benchmark]
        public async Task GetAllChannelsBenchmark()
        {
            var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Channel>()
                .GetAll();
        }

        [Benchmark]
        public async Task GetAllChannelsExtensionBenchmark()
        {
            var result = await _services.GetService<IBrokerObjectFactory>()
                .GetAllChannels();
        }
    }
}