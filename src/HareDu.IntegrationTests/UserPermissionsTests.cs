namespace HareDu.IntegrationTests
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class UserPermissionsTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            _container = new ContainerBuilder()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .Build();
        }

        [Test]
        public async Task Should_be_able_to_get_all_user_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .GetAll()
                .ScreenDump();
        }

        [Test]
        public async Task Verify_can_delete_user_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Delete(x =>
                {
                    x.User("");
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });
        }

        [Test]
        public async Task Verify_can_create_user_permissions()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<UserPermissions>()
                .Create(x =>
                {
                    x.User("");
                    x.Configure(c =>
                    {
                        c.UsingConfigurePattern("");
                        c.UsingReadPattern("");
                        c.UsingWritePattern("");
                    });
                    x.Targeting(t => t.VirtualHost("HareDu5"));
                });
        }
    }
}