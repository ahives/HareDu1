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
    public class FileDescriptorThrottlingProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test]
        public void Verify_probe_warning_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new FileDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = new FakeOperatingSystemSnapshot1(100, 90);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(FileDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_unhealthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new FileDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = new FakeOperatingSystemSnapshot1(100, 100);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Unhealthy);
            result.KB.Id.ShouldBe(typeof(FileDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_healthy_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var probe = new FileDescriptorThrottlingProbe(config.Diagnostics, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = new FakeOperatingSystemSnapshot1(100, 60);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Healthy);
            result.KB.Id.ShouldBe(typeof(FileDescriptorThrottlingProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_na()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new FileDescriptorThrottlingProbe(null, knowledgeBaseProvider);
            
            OperatingSystemSnapshot snapshot = new FakeOperatingSystemSnapshot1(100, 60);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.NA);
        }
    }
}