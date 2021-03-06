namespace HareDu.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTests
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
        public async Task Verify_can_create_queue()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create(x =>
                {
                    x.Queue("TestQueue31");
                    x.Configure(c =>
                    {
                        c.IsDurable();
                        c.HasArguments(arg =>
                        {
                            arg.SetQueueExpiration(1000);
                            arg.SetAlternateExchange("your_alternate_exchange_name");
                            arg.SetDeadLetterExchange("your_deadletter_exchange_name");
                            arg.SetPerQueuedMessageExpiration(1000);
                            arg.SetDeadLetterExchangeRoutingKey("your_routing_key");
                        });
                    });
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Should_be_able_to_get_all_queues()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .GetAll()
                .ScreenDump();

            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_get_all_json()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .GetAll();
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_delete_queue()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Delete(x =>
                {
                    x.Queue("TestQueue10");
                    x.Targeting(l => l.VirtualHost("HareDu"));
                    x.When(c =>
                    {
                        // c.HasNoConsumers();
                        // c.IsEmpty();
                    });
                });

//            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }

        [Test]
        public async Task Verify_can_empty_queue()
        {
            var result = await _container.Resolve<IBrokerObjectFactory>()
                .Object<Queue>()
                .Empty(x =>
                {
                    x.Queue("");
                    x.Targeting(t => t.VirtualHost("HareDu"));
                });
            
            Assert.IsFalse(result.HasFaulted);
            Console.WriteLine(result.ToJsonString());
        }
    }
}