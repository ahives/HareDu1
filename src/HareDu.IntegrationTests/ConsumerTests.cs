namespace HareDu.IntegrationTests
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ConsumerTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .Build();
        }

        [Test, Explicit]
        public async Task Should_be_able_to_get_all_consumers()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Consumer>()
                .GetAll()
                .ScreenDump();
        }
    }
}