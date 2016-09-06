using System;
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
            var unityContainer = new UnityContainer();
            new Bootstrapper.Bootstrapper().Run(unityContainer);

            var tcpHosting = args.Length > 0 && args[0] == "tcp";
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
                    host.AddServiceEndpoint(typeof(ISignalsWebService), new System.ServiceModel.NetTcpBinding(), string.Empty);
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
