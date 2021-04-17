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
    public class BrokerConnectivityScannerTests :
        DiagnosticScannerTestingHarness
    {
        [Test]
        public void Verify_analyzers_fired()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            
            var result = new BrokerConnectivityScanner(_probes)
                .Scan(snapshot);

            result.Count.ShouldBe(6);
            result.Count(x => x.Id == typeof(HighConnectionCreationRateProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(HighConnectionClosureRateProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(UnlimitedPrefetchCountProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(ChannelThrottlingProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(ChannelLimitReachedProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(BlockedConnectionProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            BrokerConnectivitySnapshot snapshot = null;
            
            var result = new BrokerConnectivityScanner(_probes)
                .Scan(snapshot);

            result.ShouldBeEmpty();
        }
    }
}