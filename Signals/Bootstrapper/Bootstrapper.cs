using DataAccess;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Practices.Unity;
using WebService;

namespace Bootstrapper
{
    public class Bootstrapper
    {
        public IUnityContainer UnityContainer { get; private set; }

        public void Run(IUnityContainer unityContainer)
        {
            UnityContainer = unityContainer;

            SetupDataAccess();

            SetupDomain();

            SetupWebService();
        }

        public void SetupDataAccess()
        {
            UnityContainer.RegisterType<IUnitOfWorkProvider, UnitOfWorkProvider>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<ISessionProvider, UnitOfWorkProvider>(new ContainerControlledLifetimeManager());
        }

        public void SetupDomain()
        {
            UnityContainer.RegisterType<ISignalRepository, DataAccess.Repositories.SignalRepository>();

            UnityContainer.RegisterType<ISignalsDomainService, Domain.Services.Implementation.SignalsDomainService>();
        }

        public void SetupWebService()
        {
            UnityContainer.RegisterType<ISignalsWebService, SignalsWebService>();
        }
    }
}
