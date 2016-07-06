using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Infrastructure;

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
        public virtual int OlderDataSampleCountNeeded { get { return 0; } }

        [NHibernateIgnore]
        public virtual int NewerDataSampleCountNeeded { get { return 0; } }

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
    }

    public abstract class MissingValuePolicy<T> : MissingValuePolicyBase
    {
        [NHibernateIgnore]
        public override Type NativeDataType { get { return typeof(T); } }

        public abstract IEnumerable<Datum<T>> FillMissingData(
            TimeEnumerator timeEnumerator,
            IEnumerable<Datum<T>> readData,
            IEnumerable<Datum<T>> additionalOlderData,
            IEnumerable<Datum<T>> additionalNewerData);
    }
}