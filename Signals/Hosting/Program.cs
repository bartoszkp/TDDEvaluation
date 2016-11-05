using System;
using System.Linq;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;
using Unity.Wcf;
using WebService;

namespace Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpHosting = args.Any(a => a == "tcp");
            var inMemoryDatabase = args.Any(a => a == "inMemoryDatabase");
            var unityContainer = new UnityContainer();
            new Bootstrapper.Bootstrapper().Run(unityContainer, inMemoryDatabase);
            
            Uri baseAddress = tcpHosting
                ? new Uri("net.tcp://localhost:8080/signals")
                : new Uri("http://localhost:8080/signals");

            using (var host = new UnityServiceHost(unityContainer, typeof(SignalsWebService), baseAddress))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = !tcpHosting;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                host.Description.Behaviors.Add(smb);

                if (tcpHosting)
                {
                    var binding = new System.ServiceModel.NetTcpBinding();
                    binding.MaxBufferSize = int.MaxValue;
                    binding.MaxReceivedMessageSize = int.MaxValue;
                    host.AddServiceEndpoint(typeof(ISignalsWebService), binding, string.Empty);
                }

                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
