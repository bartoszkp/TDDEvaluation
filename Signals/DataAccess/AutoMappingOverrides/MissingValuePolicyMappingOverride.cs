using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DataAccess.AutoMappingOverrides
{
    public class MissingValuePolicyMappingOverride : IAutoMappingOverride<Domain.MissingValuePolicy.MissingValuePolicy>
    {
        public void Override(AutoMapping<Domain.MissingValuePolicy.MissingValuePolicy> mapping)
        {
            mapping
                .Id(m => m.Id)
                .GeneratedBy
                .Increment();

            mapping
                .References(mvp => mvp.Signal)
                .Unique()
                .Cascade
                .All();
        }
    }
}
