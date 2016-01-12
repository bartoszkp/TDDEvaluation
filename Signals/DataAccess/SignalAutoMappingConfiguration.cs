using System;
using FluentNHibernate;

namespace DataAccess
{
    public class SignalAutoMappingConfiguration : FluentNHibernate.Automapping.DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return base.ShouldMap(type) && type.Namespace == "Domain";
        }

        public override bool IsComponent(Type type)
        {
            return type.IsDefined(typeof(Domain.Infrastructure.ComponentAttribute), false);
        }
    }
}
