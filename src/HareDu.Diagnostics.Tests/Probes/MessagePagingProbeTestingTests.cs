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
    public class MessagePagingProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test]
        public void Verify_analyzer_unhealthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new MessagePagingProbe(knowledgeBaseProvider);

            QueueSnapshot snapshot = new FakeQueueSnapshot4(3);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(MessagePagingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_analyzer_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new MessagePagingProbe(knowledgeBaseProvider);
            
            QueueSnapshot snapshot = new FakeQueueSnapshot4(0);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(MessagePagingProbe).GetIdentifier());
        }
    }
}