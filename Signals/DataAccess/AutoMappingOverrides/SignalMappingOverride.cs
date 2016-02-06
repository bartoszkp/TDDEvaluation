using Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DataAccess.AutoMappingOverrides
{
    public class SignalMappingOverride : IAutoMappingOverride<Signal>
    {
        public void Override(AutoMapping<Signal> mapping)
        {
            mapping
                .HasOne(s => s.MissingValuePolicyConfig).Cascade.All();
        }
    }
}
