using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

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

            SetupDtoAutoMapping();

            SetupUnityContainer();
        }

        public void SetupDtoAutoMapping()
        {
            Dto.Conversions.AutoMappingConfiguration.Run();
        }

        public void SetupUnityContainer()
        {
            UnityContainer.RegisterTypes(
                AllClasses.FromAssembliesInBasePath().Where(HasUnityRegisterAttribute),
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
