namespace HareDu.Snapshotting.Tests
{
    using System.Threading.Tasks;
    using CoreIntegration;
    using Fakes;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class SnapshotFactoryTests
    {
        ServiceProvider _provider;

        [OneTimeSetUp]
        public void Init()
        {
            _provider = new ServiceCollection()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .BuildServiceProvider();
        }

        [Test]
        public async Task Verify_can_return_BrokerConnection_snapshot()
        {
            var factory = _provider.GetService<ISnapshotFactory>();
            var lens = factory.Lens<BrokerConnectivitySnapshot>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<BrokerConnectivitySnapshot>>();
        }

        [Test]
        public async Task Verify_can_return_BrokerQueues_snapshot()
        {
            var factory = _provider.GetService<ISnapshotFactory>();
            var lens = factory.Lens<BrokerQueuesSnapshot>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<BrokerQueuesSnapshot>>();
        }

        [Test]
        public async Task Verify_can_return_Cluster_snapshot()
        {
            var factory = _provider.GetService<ISnapshotFactory>();
            var lens = factory.Lens<ClusterSnapshot>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<ClusterSnapshot>>();
        }

        [Test]
        public async Task Verify_can_return_new_snapshots()
        {
            var factory = _provider.GetService<ISnapshotFactory>();
            var lens = factory
                .Register(new FakeHareDuSnapshot1Impl())
                .Lens<FakeHareDuSnapshot1>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<FakeHareDuSnapshot1>>();
        }

        [Test]
        public void Verify_snapshot_not_implemented_does_not_throw()
        {
            var factory = _provider.GetService<ISnapshotFactory>();
            var lens = factory.Lens<FakeHareDuSnapshot2>();

            lens.ShouldNotBeNull();
            lens.ShouldBeAssignableTo<SnapshotLens<FakeHareDuSnapshot2>>();
        }
    }
}