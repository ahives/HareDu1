namespace HareDu.Diagnostics.Tests.Scanners
{
    using Core.Extensions;
    using CoreIntegration;
    using Diagnostics.Scanners;
    using Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class DiagnosticScannerTests
    {
        ServiceProvider _service;

        [OneTimeSetUp]
        public void Init()
        {
            _service = new ServiceCollection()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .BuildServiceProvider();
        }

        [Test]
        public void Verify_can_select_broker_connectivity_scanner()
        {
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot1();
            var result = _service.GetService<IScanner>()
                .Scan(snapshot);
            
            result.ScannerId.ShouldBe(typeof(BrokerConnectivityScanner).GetIdentifier());
        }

        [Test]
        public void Verify_can_select_cluster_scanner()
        {
            ClusterSnapshot snapshot = new FakeClusterSnapshot1();
            var result = _service.GetService<IScanner>()
                .Scan(snapshot);
            
            result.ScannerId.ShouldBe(typeof(ClusterScanner).GetIdentifier());
        }

        [Test]
        public void Verify_can_select_broker_queues_scanner()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            var result = _service.GetService<IScanner>()
                .Scan(snapshot);
            
            result.ScannerId.ShouldBe(typeof(BrokerQueuesScanner).GetIdentifier());
        }

        [Test]
        public void Verify_does_not_throw_when_scanner_not_found()
        {
            BrokerQueuesSnapshot snapshot = new FakeBrokerQueuesSnapshot1(1);
            IScannerFactory factory = new FakeScannerFactory();
            IScanner result = new Scanner(factory);

            var report = result.Scan(snapshot);
            
            report.ScannerId.ShouldBe(typeof(NoOpScanner<EmptySnapshot>).GetIdentifier());
            report.ShouldBe(DiagnosticCache.EmptyScannerResult);
        }
    }
}