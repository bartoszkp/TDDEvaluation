using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Domain.Infrastructure
{
    public class NoInjectionMembers : IInjectionMembersFactory
    {
        public IEnumerable<InjectionMember> GetInjectionMembers()
        {
            return Enumerable.Empty<InjectionMember>();
        }
    }
}
