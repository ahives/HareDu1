namespace HareDu.Diagnostics.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Configuration;
    using Core.Extensions;
    using CoreIntegration;
    using Diagnostics.Probes;
    using Diagnostics.Scanners;
    using Fakes;
    using KnowledgeBase;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Model;

    [TestFixture]
    public class ScannerFactoryTests
    {
        IReadOnlyList<DiagnosticProbe> _probes;
        ServiceProvider _provider;

        [OneTimeSetUp]
        public void Init()
        {
            _provider = new ServiceCollection()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .BuildServiceProvider();

            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            
            _probes = new List<DiagnosticProbe>
            {
                new HighConnectionCreationRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new HighConnectionClosureRateProbe(config.Diagnostics, knowledgeBaseProvider),
                new UnlimitedPrefetchCountProbe(knowledgeBaseProvider),
                new ChannelThrottlingProbe(knowledgeBaseProvider),
                new ChannelLimitReachedProbe(knowledgeBaseProvider),
                new BlockedConnectionProbe(knowledgeBaseProvider)
            };
        }

        [Test]
        public void Verify_can_get_diagnostic()
        {
            // string path = $"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml";
            //
            // var configProvider = new ConfigurationProvider();
            // configProvider.TryGet(path, out HareDuConfig config);
            //
            // var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            // var diagnosticAnalyzerRegistry = new DiagnosticAnalyzerRegistry(config.Analyzer, knowledgeBaseProvider);
            // diagnosticAnalyzerRegistry.RegisterAll();
            //
            // var diagnosticsRegistrar = new ComponentDiagnosticRegistry(diagnosticAnalyzerRegistry.ObjectCache);
            // diagnosticsRegistrar.RegisterAll();
            //
            // var factory = new ComponentDiagnosticFactory(diagnosticsRegistrar.ObjectCache, diagnosticsRegistrar.Types, _analyzers);
            var factory = _provider.GetService<IScannerFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out var diagnostic).ShouldBeTrue();
            diagnostic.Metadata.Identifier.ShouldBe(typeof(BrokerConnectivityScanner).GetIdentifier());
        }

        [Test]
        public void Test()
        {
            var factory = _provider.GetService<IScannerFactory>();

            factory.TryGet<BrokerConnectivitySnapshot>(out _).ShouldBeTrue();
//            Assert.AreEqual(typeof(BrokerConnectivityDiagnostic).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        [Test]
        public void Verify_TryGet_does_not_throw()
        {
            var factory = _provider.GetService<IScannerFactory>();

            factory.TryGet<ConnectionSnapshot>(out var diagnostic).ShouldBeFalse();
            diagnostic.Metadata.Identifier.ShouldBe(typeof(NoOpScanner<ConnectionSnapshot>).GetIdentifier());
        }

        [Test]
        public void Verify_can_get_diagnostic_after_instantiation()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var factory = new ScannerFactory(config, knowledgeBaseProvider);

            factory.TryGet<FakeSnapshot>(out _).ShouldBeFalse();
//            Assert.AreEqual(typeof(DoNothingDiagnostic<ConnectionSnapshot>).FullName.GenerateIdentifier(), diagnostic.Identifier);
        }

        // [Test]
        // public void Verify_can_add_new_probes()
        // {
        //     var provider = new YamlFileConfigProvider();
        //     provider.TryGet($"{TestContext.CurrentContext.TestDirectory}/haredu_1.yaml", out HareDuConfig config);
        //
        //     var factory = new ScannerFactory(config, new KnowledgeBaseProvider());
        //     
        //     factory.Probes.ShouldNotBeNull();
        //     factory.Probes.ShouldNotBeEmpty();
        //     factory.Probes.Keys.Count().ShouldBe(21);
        //
        //     bool registered = factory.RegisterProbe(new FakeProbe(5, _container.Resolve<IKnowledgeBaseProvider>()));
        //     registered.ShouldBeTrue();
        //     
        //     factory.Probes.ShouldNotBeNull();
        //     factory.Probes.ShouldNotBeEmpty();
        //     factory.Probes.Keys.Count().ShouldBe(22);
        // }

        [Test]
        public void Verify_can_return_all_probes_1()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var factory = new ScannerFactory(config, knowledgeBaseProvider);
            
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_probes_2()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var factory = new ScannerFactory(config, knowledgeBaseProvider);

            bool registered = factory.TryRegisterAllProbes();
            
            registered.ShouldBeTrue();
            factory.Probes.ShouldNotBeNull();
            factory.Probes.ShouldNotBeEmpty();
            factory.Probes.Keys.Count().ShouldBe(21);
        }

        [Test]
        public void Verify_can_return_all_scanners_1()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var factory = new ScannerFactory(config, knowledgeBaseProvider);
            
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_return_all_scanners_2()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var factory = new ScannerFactory(config, knowledgeBaseProvider);

            bool registered = factory.TryRegisterAllScanners();
            
            registered.ShouldBeTrue();
            factory.Scanners.ShouldNotBeNull();
            factory.Scanners.ShouldNotBeEmpty();
            factory.Scanners.Keys.Count().ShouldBe(3);
        }

        [Test]
        public void Verify_can_add_new_probe()
        {
            var knowledgeBaseProvider = _provider.GetService<IKnowledgeBaseProvider>();
            var config = _provider.GetService<HareDuConfig>();
            var factory = _provider.GetService<IScannerFactory>();

            bool registered = factory.TryRegisterProbe(new FakeProbe(knowledgeBaseProvider));
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(registered);
                Assert.AreEqual(typeof(FakeProbe).GetIdentifier(), factory.Probes[typeof(FakeProbe).FullName].Metadata.Id);
            });
        }

        [Test]
        public void Verify_can_add_scanner()
        {
            var factory = _provider.GetService<IScannerFactory>();

            bool registered = factory.TryRegisterScanner(new FakeDiagnosticScanner());

            Assert.Multiple(() =>
            {
                Assert.IsTrue(registered);
                
                var scanner = (FakeDiagnosticScanner)factory.Scanners[typeof(FakeDiagnosticScanner).FullName];
                
                Assert.AreEqual(typeof(FakeDiagnosticScanner).GetIdentifier(), scanner.Metadata.Identifier);
            });
        }
    }
}