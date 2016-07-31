using System.Collections.Generic;
using Domain.Infrastructure;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace DataAccess.Infrastructure
{
    public class DatabaseTransactionInterceptionInjectionMembersFactory : IInjectionMembersFactory
    {
        public IEnumerable<InjectionMember> GetInjectionMembers()
        {
            return new InjectionMember[] { new Interceptor<TransparentProxyInterceptor>(), new InterceptionBehavior<DatabaseTransactionInterception>() };
        }
    }
}
