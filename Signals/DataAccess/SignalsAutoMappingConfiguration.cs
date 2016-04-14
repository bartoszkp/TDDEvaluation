﻿using System;

namespace DataAccess
{
    public class SignalsAutoMappingConfiguration : FluentNHibernate.Automapping.DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return base.ShouldMap(type)
                && !type.IsDefined(typeof(Domain.Infrastructure.NHibernateIgnoreAttribute), false)
                && !type.ContainsGenericParameters
                && (type.Namespace == "Domain" || type.Namespace == "DataAccess.GenericInstantiations");
        }

        public override bool IsComponent(Type type)
        {
            return type.IsDefined(typeof(Domain.Infrastructure.ComponentAttribute), false);
        }
    }
}
