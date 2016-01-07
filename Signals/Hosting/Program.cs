using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;

namespace Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            var unityContainer = new UnityContainer();
            new Bootstrapper.Bootstrapper().Run(unityContainer);

            Uri baseAddress = new Uri("http://localhost:8080/signals");

            var serviceInstance = unityContainer.Resolve<WebService.ISignalsWebService>();

            using (var host = new ServiceHost(serviceInstance, baseAddress))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
