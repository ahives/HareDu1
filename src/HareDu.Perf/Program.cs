namespace HareDu.Perf
{
    using BenchmarkDotNet.Running;
    using Benchmarks;

    class Program
    {
        public static void Main(string[] args)
        {
            // var run = BenchmarkRunner.Run(typeof(Program).Assembly);
            var run = BenchmarkRunner.Run<GetAllChannelBenchmarks>();
        }
    }
}