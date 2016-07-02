using Domain.Infrastructure; // TODO ugly dependency, change iface to DateTime
using System;
using System.Collections.Generic;

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
    }

    public abstract class MissingValuePolicy<T> : MissingValuePolicyBase
    {
        [NHibernateIgnore]
        public override Type NativeDataType { get { return typeof(T); } }

        [NHibernateIgnore]
        public virtual int OlderDataSamplesCountNeeded { get { return 0; } }

        public abstract IEnumerable<Datum<T>> FillMissingData(TimeEnumerator timeEnumerator,
                                                                      IEnumerable<Datum<T>> readData,
                                                                      IEnumerable<Datum<T>> additionalOlderData);
    }
}