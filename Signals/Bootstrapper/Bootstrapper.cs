﻿using DataAccess;
using DataAccess.Infrastructure;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using WebService;

namespace Bootstrapper
{
    public class Bootstrapper
    {
        public IUnityContainer UnityContainer { get; private set; }

        public void Run(IUnityContainer unityContainer)
        {
            UnityContainer = unityContainer;
            UnityContainer.AddNewExtension<Interception>();

            UnityContainer.RegisterInstance<IUnityContainer>(UnityContainer, new ExternallyControlledLifetimeManager());

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
            UnityContainer.RegisterType<ISignalsWebService, SignalsWebService>(
                new Interceptor<TransparentProxyInterceptor>(),
                new InterceptionBehavior<DatabaseTransactionInterception>());
        }
    }
}
