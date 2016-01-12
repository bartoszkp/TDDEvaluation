using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace WebService.Infrastructure
{
    public class UnityServiceBehavior : IServiceBehavior
    {
        private readonly IUnityContainer unityContainer;

        public UnityServiceBehavior(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    if (endpointDispatcher.ContractName != "IMetadataExchange")
                    {
                        var serviceEndpoint = serviceDescription
                            .Endpoints
                            .FirstOrDefault(e => e.Contract.Name == endpointDispatcher.ContractName);

                        endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(this.unityContainer, serviceEndpoint.Contract.ContractType);
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
