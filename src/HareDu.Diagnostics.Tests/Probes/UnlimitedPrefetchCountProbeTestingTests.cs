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
    public class UnlimitedPrefetchCountProbeTestingTests :
        DiagnosticProbeTestingHarness
    {
        [Test(Description = "")]
        public void Verify_probe_warning_condition()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new UnlimitedPrefetchCountProbe(knowledgeBaseProvider);

            ChannelSnapshot snapshot = new FakeChannelSnapshot2(0);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Warning);
            result.KB.Id.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }

        [Test]
        public void Verify_probe_inconclusive_condition_1()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var probe = new UnlimitedPrefetchCountProbe(knowledgeBaseProvider);
            
            ChannelSnapshot snapshot = new FakeChannelSnapshot2(5);

            var result = probe.Execute(snapshot);
            
            result.Status.ShouldBe(ProbeResultStatus.Inconclusive);
            result.KB.Id.ShouldBe(typeof(UnlimitedPrefetchCountProbe).GetIdentifier());
        }
    }
}