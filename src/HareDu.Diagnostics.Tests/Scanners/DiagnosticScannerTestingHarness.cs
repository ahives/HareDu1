namespace HareDu.Diagnostics.Tests.Scanners
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Internal;
    using Diagnostics.Probes;
    using KnowledgeBase;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class DiagnosticScannerTestingHarness
    {
        protected IReadOnlyList<DiagnosticProbe> _probes;
        protected ServiceProvider _provider;

        [OneTimeSetUp]
        public void Init()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton<IKnowledgeBaseProvider, KnowledgeBaseProvider>();

            var internalConfig = new InternalHareDuConfig();
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", false)
                .Build();

            configuration.Bind("HareDuConfig", internalConfig);

            HareDuConfig config = ConfigMapper.Map(internalConfig);
            
            services.AddSingleton(config);
            
            _provider = services.BuildServiceProvider();

            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            
            _probes = new List<DiagnosticProbe>
            {
                new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new HighConnectionClosureRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new UnlimitedPrefetchCountProbe(knowledgeBaseProvider),
                new ChannelThrottlingProbe(knowledgeBaseProvider),
                new ChannelLimitReachedProbe(knowledgeBaseProvider),
                new BlockedConnectionProbe(knowledgeBaseProvider),
                new QueueGrowthProbe(knowledgeBaseProvider),
                new MessagePagingProbe(knowledgeBaseProvider),
                new RedeliveredMessagesProbe(config.Diagnostics, knowledgeBaseProvider),
                new ConsumerUtilizationProbe(config.Diagnostics, knowledgeBaseProvider),
                new UnroutableMessageProbe(knowledgeBaseProvider),
                new QueueLowFlowProbe(config.Diagnostics, knowledgeBaseProvider),
                new QueueNoFlowProbe(knowledgeBaseProvider),
                new QueueHighFlowProbe(config.Diagnostics, knowledgeBaseProvider),
                new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider),
                new SocketDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider),
                new NetworkPartitionProbe(knowledgeBaseProvider),
                new MemoryAlarmProbe(knowledgeBaseProvider),
                new DiskAlarmProbe(knowledgeBaseProvider),
                new AvailableCpuCoresProbe(knowledgeBaseProvider),
                new FileDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider)
            };
        }
    }
}