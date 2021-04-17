namespace HareDu.Diagnostics.Tests.Probes
{
    using Core.Extensions;
    using Diagnostics.Probes;
    using Fakes;
    using KnowledgeBase;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class MemoryAlarmProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test]
        public void Verify_probe_unhealthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new MemoryAlarmProbe(knowledgeBaseProvider);
            
            MemorySnapshot snapshot = new FakeMemorySnapshot1(103283, 823983, true);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(MemoryAlarmProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new MemoryAlarmProbe(knowledgeBaseProvider);
            
            MemorySnapshot snapshot = new FakeMemorySnapshot1(103283, 823983, false);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(MemoryAlarmProbe).GetIdentifier());
        }
    }
}