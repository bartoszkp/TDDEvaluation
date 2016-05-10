using System;
using FluentNHibernate;

namespace DataAccess
{
    public class SignalsAutoMappingConfiguration : FluentNHibernate.Automapping.DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return base.ShouldMap(type)
                && !type.IsDefined(typeof(Domain.Infrastructure.NHibernateIgnoreAttribute), false)
                && !type.ContainsGenericParameters
                && (type.Namespace.StartsWith("Domain") || type.Namespace == "DataAccess.GenericInstantiations")
                && !(type.Namespace == "Domain.Infrastructure" || type.Namespace == "Domain.Exceptions" || type.Namespace == "Repositories" || type.Namespace.StartsWith("Domain.Services"));
        }

        public override bool ShouldMap(Member member)
        {
            return base.ShouldMap(member)
                && !member.MemberInfo.IsDefined(typeof(Domain.Infrastructure.NHibernateIgnoreAttribute), false);
        }

        public override bool IsComponent(Type type)
        {
            return type.IsDefined(typeof(Domain.Infrastructure.ComponentAttribute), false);
        }
    }
}
