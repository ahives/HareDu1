namespace HareDu.Diagnostics.Tests.Probes
{
    using Core.Extensions;
    using Diagnostics.Probes;
    using Fakes;
    using KnowledgeBase;
    using Model;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class BlockedConnectionProbeTestingTest :
        DiagnosticProbeTestingHarness
    {
        [Test]
        public void Verify_probe_unhealthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new BlockedConnectionProbe(knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot2(BrokerConnectionState.Blocked);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(BlockedConnectionProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new BlockedConnectionProbe(knowledgeBaseProvider);
            
            ConnectionSnapshot snapshot = new FakeConnectionSnapshot2(BrokerConnectionState.Running);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(BlockedConnectionProbe).GetIdentifier());
        }
    }
}