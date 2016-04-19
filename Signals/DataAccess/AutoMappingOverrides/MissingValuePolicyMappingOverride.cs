using Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DataAccess.AutoMappingOverrides
{
    public class MissingValuePolicyMappingOverride : IAutoMappingOverride<MissingValuePolicy>
    {
        public void Override(AutoMapping<MissingValuePolicy> mapping)
        {
            mapping
                .References(mvp => mvp.Signal)
                .Unique()
                .Cascade
                .All();
        }
    }
}
