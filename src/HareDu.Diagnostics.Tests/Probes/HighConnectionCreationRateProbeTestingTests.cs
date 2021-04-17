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
    public class HighConnectionCreationRateProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test]
        public void Verify_probe_warning_condition_1()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(102, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(HighConnectionCreationRateProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_warning_condition_2()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(100, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(HighConnectionCreationRateProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(99, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(HighConnectionCreationRateProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new HighConnectionCreationRateProbe(null, knowledgeBaseProvider);
            
            BrokerConnectivitySnapshot snapshot = new FakeBrokerConnectivitySnapshot2(99, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
        }
    }
}