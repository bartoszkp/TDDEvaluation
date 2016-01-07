using System;
using FluentNHibernate;

namespace DataAccess
{
    public class SignalAutoMappingConfiguration : FluentNHibernate.Automapping.DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "Domain";
        }

        public override bool IsComponent(Type type)
        {
            return type.Name == "Path" || type.Name == "Datum`1" || type.Name == "DataType" || type.Name == "Granularity";
        }

        public override bool IsConcreteBaseType(Type type)
        {
            return base.IsConcreteBaseType(type);
        }
    }
}
