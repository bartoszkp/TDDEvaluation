using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Domain.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UnityRegisterAttribute : Attribute
    {
        public LifetimeManager LifetimeManager { get; private set; }

        public IEnumerable<InjectionMember> InjectionMembers { get; private set; }

        public UnityRegisterAttribute()
            : this(typeof(TransientLifetimeManager), typeof(NoInjectionMembers))
        {
        }

        public UnityRegisterAttribute(Type lifetimeManager)
            : this(lifetimeManager, typeof(NoInjectionMembers))
        {
        }

        public UnityRegisterAttribute(Type lifetimeManager, Type injectionMembersFactory)
        {
            LifetimeManager = lifetimeManager.GetConstructor(Type.EmptyTypes).Invoke(null) as LifetimeManager;
            InjectionMembers = (injectionMembersFactory.GetConstructor(Type.EmptyTypes).Invoke(null)
                as IInjectionMembersFactory)
                .GetInjectionMembers();
        }
    }
}
