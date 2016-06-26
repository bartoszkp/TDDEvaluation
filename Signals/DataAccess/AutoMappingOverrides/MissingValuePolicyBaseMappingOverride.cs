using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DataAccess.AutoMappingOverrides
{
    public class MissingValuePolicyBaseMappingOverride
        : IAutoMappingOverride<Domain.MissingValuePolicy.MissingValuePolicyBase>
    {
        public void Override(AutoMapping<Domain.MissingValuePolicy.MissingValuePolicyBase> mapping)
        {
            mapping
                .Id(mvp => mvp.Id)
                .GeneratedBy
                .Increment();

            mapping
                .References(mvp => mvp.Signal)
                .Not
                .Nullable();
        }
    }
}
