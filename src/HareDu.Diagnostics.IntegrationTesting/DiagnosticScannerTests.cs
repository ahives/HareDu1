namespace HareDu.Diagnostics.IntegrationTesting
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Core.Configuration;
    using Core.Extensions;
    using CoreIntegration;
    using Formatting;
    using KnowledgeBase;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Probes;
    using Snapshotting;
    using Snapshotting.Model;

    [TestFixture]
    public class DiagnosticScannerTests
    {
        [Test]
        public async Task Test1()
        {
            var provider = new ServiceCollection()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .BuildServiceProvider();

            var scanner = provider.GetService<IScanner>();

            var lens = provider.GetService<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();
            
            var result = await lens.TakeSnapshot();
            
            var report = scanner.Scan(result.Snapshot);

            var formatter = provider.GetService<IDiagnosticReportFormatter>();
            
            string formattedReport = formatter.Format(report);
             
            Console.WriteLine(formattedReport);
        }

        [Test]
        public async Task Test3()
        {
            var provider = new ServiceCollection()
                .AddHareDu($"{TestContext.CurrentContext.TestDirectory}/appsettings.json", "HareDuConfig")
                .BuildServiceProvider();
            
            var lens = provider.GetService<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>();
            var result = await lens.TakeSnapshot();
            
            var scanner = provider.GetService<IScanner>();

            var report = scanner.Scan(result.Snapshot);

            var formatter = provider.GetService<IDiagnosticReportFormatter>();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
            
//            for (int i = 0; i < report.Results.Count; i++)
//            {
//                Console.WriteLine("Diagnostic => Channel: {0}, Status: {1}", report.Results[i].ComponentIdentifier, report.Results[i].Status);
//                
//                if (report.Results[i].Status == DiagnosticStatus.Red)
//                {
//                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Reason);
//                    Console.WriteLine(report.Results[i].KnowledgeBaseArticle.Remediation);
//                }
//            }
        }
        
        [Test]
        public async Task Test6()
        {
            var provider = new HareDuConfigProvider();

            var config1 = provider.Configure(x =>
            {
                x.Broker(y =>
                {
                    y.ConnectTo("http://localhost:15672");
                    y.UsingCredentials("guest", "guest");
                });

                x.Diagnostics(y =>
                {
                    y.Probes(z =>
                    {
                        z.SetMessageRedeliveryThresholdCoefficient(0.60M);
                        z.SetSocketUsageThresholdCoefficient(0.60M);
                        z.SetConsumerUtilizationThreshold(0.65M);
                        z.SetQueueHighFlowThreshold(90);
                        z.SetQueueLowFlowThreshold(10);
                        z.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                        z.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                        z.SetHighConnectionClosureRateThreshold(90);
                        z.SetHighConnectionCreationRateThreshold(60);
                    });
                });
            });
            
            var factory1 = new SnapshotFactory(config1);
            var lens = factory1.Lens<BrokerConnectivitySnapshot>();

            var result = await lens.TakeSnapshot();
            
            var factory2 = new ScannerFactory(config1, new KnowledgeBaseProvider());
            IScanner scanner = new Scanner(factory2);

            var report = scanner.Scan(result.Snapshot);

            var formatter = new DiagnosticReportTextFormatter();

            string formattedReport = formatter.Format(report);
            
            Console.WriteLine(formattedReport);
        }
    }
}