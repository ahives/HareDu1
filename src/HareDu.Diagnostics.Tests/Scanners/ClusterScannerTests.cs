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
    public class ClusterScannerTests :
        DiagnosticScannerTestingHarness
    {
        [Test]
        public void Verify_analyzers_fired()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            
            var result = new ClusterScanner(_probes)
                .Scan(snapshot);

            result.Count.ShouldBe(7);
            result.Count(x => x.Id == typeof(RuntimeProcessLimitProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(SocketDescriptorThrottlingProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(NetworkPartitionProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(MemoryAlarmProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(DiskAlarmProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(AvailableCpuCoresProbe).GetIdentifier()).ShouldBe(1);
            result.Count(x => x.Id == typeof(FileDescriptorThrottlingProbe).GetIdentifier()).ShouldBe(1);
        }

        [Test]
        public void Verify_empty_result_returned_when_snapshot_null()
        {
            ClusterSnapshot snapshot = null;
            
            var result = new ClusterScanner(_probes)
                .Scan(snapshot);

            result.ShouldBeEmpty();
        }
    }
}