namespace HareDu.Diagnostics.Tests.Probes
{
    using System.Collections.Generic;
    using Core.Extensions;
    using Diagnostics.Probes;
    using Fakes;
    using KnowledgeBase;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class NetworkPartitionProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test]
        public void Verify_probe_unhealthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new NetworkPartitionProbe(knowledgeBaseProvider);

            NodeSnapshot snapshot = new FakeNodeSnapshot2(new List<string>
            {
                "node1@rabbitmq",
                "node2@rabbitmq",
                "node3@rabbitmq"
            });

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(NetworkPartitionProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new NetworkPartitionProbe(knowledgeBaseProvider);
            
            NodeSnapshot snapshot = new FakeNodeSnapshot2(new List<string>());

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(NetworkPartitionProbe).GetIdentifier());
        }
    }
}