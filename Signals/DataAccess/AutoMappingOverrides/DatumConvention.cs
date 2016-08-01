using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace DataAccess.AutoMappingOverrides
{
    public class DatumConvention : IPropertyConvention, IPropertyConventionAcceptance, IReferenceConvention, IReferenceConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IManyToOneInspector> criteria)
        {
            AcceptDatumInstantiations(criteria);

            criteria.Expect(c => c.Property.PropertyType.Equals(typeof(Domain.Signal)));
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            AcceptDatumInstantiations(criteria);

            criteria.Expect(c => c.Type.Equals(typeof(DateTime)));
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.UniqueKey(GetConstraintName(instance.EntityType));
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.UniqueKey(GetConstraintName(instance.EntityType));
        }

        private void AcceptDatumInstantiations<T>(IAcceptanceCriteria<T> criteria) where T : IInspector
        {
            criteria.Expect(c => c.EntityType.BaseType.IsGenericType
                   && c.EntityType.BaseType.GetGenericTypeDefinition().Equals(typeof(Domain.Datum<>)));
        }

        private string GetConstraintName(Type entityType)
        {
            return entityType.Name + "Signal_Timestamp_Unique";
        }
    }
}
