namespace HareDu.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class GlobalParameterTests
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
        public async Task Should_be_able_to_get_all_global_parameters()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .GetAll()
                .ScreenDump();
        }
        
        [Test]
        public async Task Verify_can_create_parameter()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Create(x =>
                {
                    x.Parameter("fake_param2");
                    x.Value("fake_value");
//                    x.Arguments(arg =>
//                    {
//                        arg.Set("arg1", "value1");
//                        arg.Set("arg2", "value2");
//                    });
                });
             
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine((string) result.ToJsonString());
        }
        
        [Test]
        public async Task Verify_can_delete_parameter()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<GlobalParameter>()
                .Delete(x => x.Parameter("Fred"));
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine((string) result.ToJsonString());
        }
    }
}