using System;

namespace Domain
{
    public abstract class DatumBase
    {
        public static DatumBase CreateNone(Type nativeDataType, Signal signal, DateTime timestamp)
        {
            var createNone = Infrastructure
                .ReflectionUtils
                .GetMethodInfo<Datum<object>>(x => Datum<object>.CreateNone(null, DateTime.MinValue));

            var concreteDatum = typeof(Datum<>)
                .MakeGenericType(nativeDataType);

            var concreteCreateNone = concreteDatum
                .GetMethod(createNone.Name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            return concreteCreateNone
                .Invoke(null, new object[] { signal, timestamp })
                as DatumBase;
        }

        public virtual int Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual Quality Quality { get; set; }

        [Infrastructure.NHibernateIgnore]
        public abstract object AbstractValue { get; }

        [Infrastructure.NHibernateIgnore]
        public abstract Type NativeDataType { get; }
    }
}
