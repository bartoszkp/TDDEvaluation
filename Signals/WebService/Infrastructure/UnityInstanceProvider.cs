using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace WebService.Infrastructure
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer unityContainer;
        private readonly Type contractType;

        public UnityInstanceProvider(IUnityContainer unityContainer, Type contractType)
        {
            this.unityContainer = unityContainer;
            this.contractType = contractType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.unityContainer.Resolve(contractType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            this.unityContainer.Teardown(instance);
        }
    }
}
