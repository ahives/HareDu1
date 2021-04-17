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
    public class RuntimeProcessLimitProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test(Description = "")]
        public void Verify_probe_unhealthy_condition_1()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test(Description = "")]
        public void Verify_probe_unhealthy_condition_2()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);

            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(3, 4, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(100, 40, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_warning_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new RuntimeProcessLimitProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(4, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new RuntimeProcessLimitProbe(null, knowledgeBaseProvider);
            
            BrokerRuntimeSnapshot snapshot = new FakeBrokerRuntimeSnapshot1(4, 3, 3.2M);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
            result.KB.Id.ShouldBe(typeof(RuntimeProcessLimitProbe).GetIdentifier());
        }
    }
}