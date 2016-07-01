using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Domain.Infrastructure
{
    public interface IInjectionMembersFactory
    {
        IEnumerable<InjectionMember> GetInjectionMembers();
    }
}
