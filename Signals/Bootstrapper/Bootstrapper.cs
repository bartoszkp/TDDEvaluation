using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Domain.Infrastructure;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Bootstrapper
{
    public class Bootstrapper
    {
        public IUnityContainer UnityContainer { get; private set; }

        public void Run(IUnityContainer unityContainer, bool inMemoryDatabase = false)
        {
            UnityContainer = unityContainer;
            UnityContainer.AddNewExtension<Interception>();

            UnityContainer.RegisterInstance<IUnityContainer>(UnityContainer, new ExternallyControlledLifetimeManager());
            UnityContainer.RegisterInstance<DataAccess.IDatabaseConfigurationProvider>(
                new DataAccess.DatabaseConfigurationProvider(inMemoryDatabase),
                new ContainerControlledLifetimeManager());

            SetupDtoAutoMapping();

            SetupUnityContainer();

            if (inMemoryDatabase)
            {
                SetupInMemoryDatabase();
            }
        }

        private void SetupInMemoryDatabase()
        {
            var uowp = UnityContainer.Resolve<IUnitOfWorkProvider>();
            using (var unitOfWork = uowp.OpenUnitOfWork())
            {
                (new DatabaseMaintenance.DatabaseMaintenance(UnityContainer.Resolve<ISessionProvider>()))
                    .RebuildDatabase();

                unitOfWork.Commit();
            }
        }

        public void SetupDtoAutoMapping()
        {
            Dto.Conversions.AutoMappingConfiguration.Run();
        }

        public void SetupUnityContainer()
        {
            UnityContainer.RegisterTypes(
                Unity.WebApi.AllClasses.FromAssembliesInSearchPath().Where(HasUnityRegisterAttribute),
                getFromTypes: WithMappings.FromAllInterfaces,
                getLifetimeManager: GetLifetimeManager,
                getInjectionMembers: GetInjectionMembers);
        }

        private static bool HasUnityRegisterAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof(UnityRegisterAttribute), false).Any();
        }

        private static LifetimeManager GetLifetimeManager(Type type)
        {
            var registrationAttribute = type.GetCustomAttributes(typeof(UnityRegisterAttribute), false).Single()
                as UnityRegisterAttribute;

            return registrationAttribute.LifetimeManager;
        }

        private static IEnumerable<InjectionMember> GetInjectionMembers(Type type)
        {
            var registrationAttribute = type.GetCustomAttributes(typeof(UnityRegisterAttribute), false).Single()
                as UnityRegisterAttribute;

            return registrationAttribute.InjectionMembers;
        }
    }
}
