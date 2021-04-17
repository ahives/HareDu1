namespace HareDu.Diagnostics.Tests.Scanners
{
    using System.Linq;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Diagnostics.Scanners;
    using Fakes;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class BrokerQueuesScannerTests :
        DiagnosticScannerTestingHarness
    {
        [Test]
        public void Verify_analyzers_fired()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);

            var result = new BrokerQueuesScanner(_probes)
                .Scan(snapshot);

            result.Count.ShouldBe(8);
            result.Count(x => x.Id == typeof(QueueGrowthProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(MessagePagingProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(RedeliveredMessagesProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(ConsumerUtilizationProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(UnroutableMessageProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(QueueLowFlowProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(QueueNoFlowProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(QueueHighFlowProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerQueuesSnapshot snapshot = null;
            
            var result = new BrokerQueuesScanner(_probes)
                .Scan(snapshot);

            result.ShouldBeEmpty();
        }
    }
}