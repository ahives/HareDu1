namespace HareDu.Diagnostics.Tests.Probes
{
    using Core.Configuration;
    using Core.Configuration.Internal;
    using KnowledgeBase;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class DiagnosticProbeTestingHarness
    {
        protected ServiceProvider _provider;

        [OneTimeSetUp]
        public void Init()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton<IKnowledgeBaseProvider, KnowledgeBaseProvider>();

            var internalConfig = new InternalHareDuConfig();
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", false)
                .Build();

            configuration.Bind("HareDuConfig", internalConfig);

            HareDuConfig config = ConfigMapper.Map(internalConfig);

            services.AddSingleton(config);
            
            _provider = services.BuildServiceProvider();
        }
    }
}