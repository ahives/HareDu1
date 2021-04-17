namespace HareDu.Diagnostics.Tests.Probes
{
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Fakes;
    using KnowledgeBase;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class QueueLowFlowProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test]
        public void Verify_probe_unhealthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new QueueLowFlowProbe(config.Diagnostics, knowledgeBaseProvider);

            QueueSnapshot snapshot = new FakeQueueSnapshot5(20);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(QueueLowFlowProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new QueueLowFlowProbe(config.Diagnostics, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot5(100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(QueueLowFlowProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new QueueLowFlowProbe(null, knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot5(100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
            result.KB.Id.ShouldBe(typeof(QueueLowFlowProbe).GetIdentifier());
        }
    }
}