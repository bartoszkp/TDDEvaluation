using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DataAccess.AutoMappingOverrides
{
    public class NoneQualityMissingValuePolicyMappingOverride
        : IAutoMappingOverride<GenericInstantiations.NoneQualityMissingValuePolicyBoolean>,
          IAutoMappingOverride<GenericInstantiations.NoneQualityMissingValuePolicyInteger>,
          IAutoMappingOverride<GenericInstantiations.NoneQualityMissingValuePolicyDouble>,
          IAutoMappingOverride<GenericInstantiations.NoneQualityMissingValuePolicyDecimal>,
          IAutoMappingOverride<GenericInstantiations.NoneQualityMissingValuePolicyString>
    {
        public void Override(AutoMapping<GenericInstantiations.NoneQualityMissingValuePolicyBoolean> mapping)
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

        public void Override(AutoMapping<GenericInstantiations.NoneQualityMissingValuePolicyInteger> mapping)
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

        public void Override(AutoMapping<GenericInstantiations.NoneQualityMissingValuePolicyDouble> mapping)
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

        public void Override(AutoMapping<GenericInstantiations.NoneQualityMissingValuePolicyDecimal> mapping)
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

        public void Override(AutoMapping<GenericInstantiations.NoneQualityMissingValuePolicyString> mapping)
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
