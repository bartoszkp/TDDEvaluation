﻿using Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DataAccess.AutoMappingOverrides
{
    public class SignalMappingOverride : IAutoMappingOverride<Signal>
    {
        public void Override(AutoMapping<Signal> mapping)
        {
            mapping
                .Id(s => s.Id)
                .GeneratedBy
                .Increment();

            mapping
               .HasOne(s => s.MissingValuePolicy)
               .PropertyRef(m => m.Signal)
               .Cascade
               .All();
        }
    }
}
