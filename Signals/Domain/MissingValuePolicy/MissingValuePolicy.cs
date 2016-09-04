using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Infrastructure;
using Domain.Repositories;

namespace Domain.MissingValuePolicy
{
    public abstract class MissingValuePolicyBase
    {
        public static MissingValuePolicyBase CreateForNativeType(Type genericPolicyType, Type nativeType)
        {
            return genericPolicyType
                .MakeGenericType(nativeType)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null) as MissingValuePolicyBase;
        }

        public virtual int? Id { get; set; }

        public virtual Signal Signal { get; set; }

        [NHibernateIgnore]
        public abstract Type NativeDataType { get; }

        [NHibernateIgnore]
        public virtual IEnumerable<Type> CompatibleNativeTypes
        {
            get
            {
                return Enum
                    .GetValues(typeof(DataType))
                    .Cast<DataType>()
                    .Select(dt => dt.GetNativeType());
            }
        }

        [NHibernateIgnore]
        public virtual IEnumerable<Granularity> CompatibleGranularities
        {
            get
            {
                return Enum
                    .GetValues(typeof(Granularity))
                    .Cast<Granularity>();
            }
        }
    }

    public abstract class MissingValuePolicy<T> : MissingValuePolicyBase
    {
        [NHibernateIgnore]
        public override Type NativeDataType { get { return typeof(T); } }

        public abstract IEnumerable<Datum<T>> GetDataAndFillMissingSamples(
            TimeEnumerator timeEnumerator, 
            ISignalsDataRepository repository);
    }
}