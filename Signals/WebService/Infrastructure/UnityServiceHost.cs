using System;
using System.ServiceModel;
using Microsoft.Practices.Unity;

namespace WebService.Infrastructure
{
    public class UnityServiceHost : ServiceHost
    {
        private readonly IUnityContainer unityContainer;

        public UnityServiceHost(IUnityContainer unityContainer, Type serviceType)
            : base(null)
        {
            this.unityContainer = unityContainer;
        }

        protected override void OnOpening()
        {
            base.OnOpening();

            if (this.Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                this.Description.Behaviors.Add(new UnityServiceBehavior(this.unityContainer));
            }
        }
    }
}
