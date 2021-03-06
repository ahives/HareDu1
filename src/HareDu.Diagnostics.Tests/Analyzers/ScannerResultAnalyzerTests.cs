namespace HareDu.Diagnostics.Tests.Analyzers
{
    using System;
    using System.Linq;
    using Core.Extensions;
    using CoreIntegration;
    using Diagnostics.Probes;
    using Extensions;
    using Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ScannerResultAnalyzerTests
    {
        ServiceProvider _provider;

        [OneTimeSetUp]
        public void Init()
        {
            _provider = new ServiceCollection()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings1.json", "HareDuConfig")
                .BuildServiceProvider();
        }

        [Test]
        public void Verify_can_observe_analysis()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();

            var analyzer = _provider.GetService<IScannerResultAnalyzer>();
                // .RegisterObserver(new FakeScannerAnalyzerObserver());
            
            var summary = _provider.GetService<IScanner>()
                .Scan(snapshot)
                .Analyze(analyzer, x => x.ComponentType.ToString())
                .ScreenDump();
        }
        
        [Test]
        public void Verify_can_analyze_by_component_type()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();

            var analyzer = _provider.GetService<IScannerResultAnalyzer>();
            var summary = _provider.GetService<IScanner>()
                .Scan(snapshot)
                .Analyze(analyzer, x => x.ComponentType.ToString());
            
            summary.ShouldNotBeNull();
            summary.Count.ShouldBe(2);
            
            summary.Any(x => x.Id == "Queue").ShouldBeTrue();

            var queueSummary = summary
                .SingleOrDefault(x => x.Id == "Queue");
            queueSummary.ShouldNotBeNull();
            queueSummary.Healthy.Total.ShouldBe<uint>(26);
            Decimal.Round(queueSummary.Healthy.Percentage, 2).ShouldBe(46.43M);
            queueSummary.Unhealthy.Total.ShouldBe<uint>(30);
            Decimal.Round(queueSummary.Unhealthy.Percentage, 2).ShouldBe(53.57M);
            queueSummary.Warning.Total.ShouldBe<uint>(0);
            queueSummary.Warning.Percentage.ShouldBe(0);
            queueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            queueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == "Exchange").ShouldBeTrue();

            var exchangeSummary = summary
                .SingleOrDefault(x => x.Id == "Exchange");
            exchangeSummary.ShouldNotBeNull();
            exchangeSummary.Healthy.Total.ShouldBe<uint>(0);
            exchangeSummary.Healthy.Percentage.ShouldBe(0);
            exchangeSummary.Unhealthy.Total.ShouldBe<uint>(1);
            exchangeSummary.Unhealthy.Percentage.ShouldBe(100);
            exchangeSummary.Warning.Total.ShouldBe<uint>(0);
            exchangeSummary.Warning.Percentage.ShouldBe(0);
            exchangeSummary.Inconclusive.Total.ShouldBe<uint>(0);
            exchangeSummary.Inconclusive.Percentage.ShouldBe(0);
        }
        
        [Test]
        public void Verify_can_analyze_by_parent_component()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();

            var summary = _provider.GetService<IScanner>()
                .Scan(snapshot)
                .Analyze(_provider.GetService<IScannerResultAnalyzer>(), x => x.ParentComponentId);
            
            summary.Count.ShouldBe(2);
            
            summary.Any(x => x.Id == "Cluster 1").ShouldBeTrue();

            var clusterSummary = summary
                .SingleOrDefault(x => x.Id == "Cluster 1");
            clusterSummary.ShouldNotBeNull();
            clusterSummary.Healthy.Total.ShouldBe<uint>(0);
            clusterSummary.Healthy.Percentage.ShouldBe(0);
            clusterSummary.Unhealthy.Total.ShouldBe<uint>(1);
            clusterSummary.Unhealthy.Percentage.ShouldBe(100);
            clusterSummary.Warning.Total.ShouldBe<uint>(0);
            clusterSummary.Warning.Percentage.ShouldBe(0);
            clusterSummary.Inconclusive.Total.ShouldBe<uint>(0);
            clusterSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == "Node0").ShouldBeTrue();

            var nodeSummary = summary
                .SingleOrDefault(x => x.Id == "Node0");
            nodeSummary.ShouldNotBeNull();
            nodeSummary.Healthy.Total.ShouldBe<uint>(26);
            Decimal.Round(nodeSummary.Healthy.Percentage, 2).ShouldBe(46.43M);
            nodeSummary.Unhealthy.Total.ShouldBe<uint>(30);
            Decimal.Round(nodeSummary.Unhealthy.Percentage, 2).ShouldBe(53.57M);
            nodeSummary.Warning.Total.ShouldBe<uint>(0);
            nodeSummary.Warning.Percentage.ShouldBe(0);
            nodeSummary.Inconclusive.Total.ShouldBe<uint>(0);
            nodeSummary.Inconclusive.Percentage.ShouldBe(0);
        }
        
        [Test]
        public void Verify_can_analyze_by_probe()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot();

            var summary = _provider.GetService<IScanner>()
                .Scan(snapshot)
                .Analyze(_provider.GetService<IScannerResultAnalyzer>(), x => x.Id);
            
            summary.Count.ShouldBe(8);
            
            summary.Any(x => x.Id == typeof(UnroutableMessageProbe).GetIdentifier()).ShouldBeTrue();

            var unroutableSummary = summary
                .SingleOrDefault(x => x.Id == typeof(UnroutableMessageProbe).GetIdentifier());
            unroutableSummary.ShouldNotBeNull();
            unroutableSummary.Healthy.Total.ShouldBe<uint>(0);
            unroutableSummary.Healthy.Percentage.ShouldBe(0);
            unroutableSummary.Unhealthy.Total.ShouldBe<uint>(1);
            unroutableSummary.Unhealthy.Percentage.ShouldBe(100);
            unroutableSummary.Warning.Total.ShouldBe<uint>(0);
            unroutableSummary.Warning.Percentage.ShouldBe(0);
            unroutableSummary.Inconclusive.Total.ShouldBe<uint>(0);
            unroutableSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(MessagePagingProbe).GetIdentifier()).ShouldBeTrue();

            var memoryPagedOutSummary = summary
                .SingleOrDefault(x => x.Id == typeof(MessagePagingProbe).GetIdentifier());
            memoryPagedOutSummary.ShouldNotBeNull();
            memoryPagedOutSummary.Healthy.Total.ShouldBe<uint>(5);
            memoryPagedOutSummary.Healthy.Percentage.ShouldBe(62.5M);
            memoryPagedOutSummary.Unhealthy.Total.ShouldBe<uint>(3);
            memoryPagedOutSummary.Unhealthy.Percentage.ShouldBe(37.5M);
            memoryPagedOutSummary.Warning.Total.ShouldBe<uint>(0);
            memoryPagedOutSummary.Warning.Percentage.ShouldBe(0);
            memoryPagedOutSummary.Inconclusive.Total.ShouldBe<uint>(0);
            memoryPagedOutSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier()).ShouldBeTrue();

            var redeliveredMessagesSummary = summary
                .SingleOrDefault(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier());
            redeliveredMessagesSummary.ShouldNotBeNull();
            redeliveredMessagesSummary.Healthy.Total.ShouldBe<uint>(0);
            redeliveredMessagesSummary.Healthy.Percentage.ShouldBe(0);
            redeliveredMessagesSummary.Unhealthy.Total.ShouldBe<uint>(8);
            redeliveredMessagesSummary.Unhealthy.Percentage.ShouldBe(100);
            redeliveredMessagesSummary.Warning.Total.ShouldBe<uint>(0);
            redeliveredMessagesSummary.Warning.Percentage.ShouldBe(0);
            redeliveredMessagesSummary.Inconclusive.Total.ShouldBe<uint>(0);
            redeliveredMessagesSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueNoFlowProbe).GetIdentifier()).ShouldBeTrue();

            var noFlowQueueSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueNoFlowProbe).GetIdentifier());
            noFlowQueueSummary.ShouldNotBeNull();
            noFlowQueueSummary.Healthy.Total.ShouldBe<uint>(3);
            noFlowQueueSummary.Healthy.Percentage.ShouldBe(37.5M);
            noFlowQueueSummary.Unhealthy.Total.ShouldBe<uint>(5);
            noFlowQueueSummary.Unhealthy.Percentage.ShouldBe(62.5M);
            noFlowQueueSummary.Warning.Total.ShouldBe<uint>(0);
            noFlowQueueSummary.Warning.Percentage.ShouldBe(0);
            noFlowQueueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            noFlowQueueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueGrowthProbe).GetIdentifier()).ShouldBeTrue();

            var queueGrowthSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueGrowthProbe).GetIdentifier());
            queueGrowthSummary.ShouldNotBeNull();
            queueGrowthSummary.Healthy.Total.ShouldBe<uint>(8);
            queueGrowthSummary.Healthy.Percentage.ShouldBe(100);
            queueGrowthSummary.Unhealthy.Total.ShouldBe<uint>(0);
            queueGrowthSummary.Unhealthy.Percentage.ShouldBe(0);
            queueGrowthSummary.Warning.Total.ShouldBe<uint>(0);
            queueGrowthSummary.Warning.Percentage.ShouldBe(0);
            queueGrowthSummary.Inconclusive.Total.ShouldBe<uint>(0);
            queueGrowthSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueLowFlowProbe).GetIdentifier()).ShouldBeTrue();

            var lowFlowQueueSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueLowFlowProbe).GetIdentifier());
            lowFlowQueueSummary.ShouldNotBeNull();
            lowFlowQueueSummary.Healthy.Total.ShouldBe<uint>(3);
            lowFlowQueueSummary.Healthy.Percentage.ShouldBe(37.5M);
            lowFlowQueueSummary.Unhealthy.Total.ShouldBe<uint>(5);
            lowFlowQueueSummary.Unhealthy.Percentage.ShouldBe(62.5M);
            lowFlowQueueSummary.Warning.Total.ShouldBe<uint>(0);
            lowFlowQueueSummary.Warning.Percentage.ShouldBe(0);
            lowFlowQueueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            lowFlowQueueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(QueueHighFlowProbe).GetIdentifier()).ShouldBeTrue();

            var highFlowQueueSummary = summary
                .SingleOrDefault(x => x.Id == typeof(QueueHighFlowProbe).GetIdentifier());
            highFlowQueueSummary.ShouldNotBeNull();
            highFlowQueueSummary.Healthy.Total.ShouldBe<uint>(7);
            highFlowQueueSummary.Healthy.Percentage.ShouldBe(87.5M);
            highFlowQueueSummary.Unhealthy.Total.ShouldBe<uint>(1);
            highFlowQueueSummary.Unhealthy.Percentage.ShouldBe(12.5M);
            highFlowQueueSummary.Warning.Total.ShouldBe<uint>(0);
            highFlowQueueSummary.Warning.Percentage.ShouldBe(0);
            highFlowQueueSummary.Inconclusive.Total.ShouldBe<uint>(0);
            highFlowQueueSummary.Inconclusive.Percentage.ShouldBe(0);
            
            summary.Any(x => x.Id == typeof(ConsumerUtilizationProbe).GetIdentifier()).ShouldBeTrue();

            var consumerUtilizationSummary = summary
                .SingleOrDefault(x => x.Id == typeof(ConsumerUtilizationProbe).GetIdentifier());
            consumerUtilizationSummary.ShouldNotBeNull();
            consumerUtilizationSummary.Healthy.Total.ShouldBe<uint>(0);
            consumerUtilizationSummary.Healthy.Percentage.ShouldBe(0);
            consumerUtilizationSummary.Unhealthy.Total.ShouldBe<uint>(8);
            consumerUtilizationSummary.Unhealthy.Percentage.ShouldBe(100);
            consumerUtilizationSummary.Warning.Total.ShouldBe<uint>(0);
            consumerUtilizationSummary.Warning.Percentage.ShouldBe(0);
            consumerUtilizationSummary.Inconclusive.Total.ShouldBe<uint>(0);
            consumerUtilizationSummary.Inconclusive.Percentage.ShouldBe(0);
        }
    }
}