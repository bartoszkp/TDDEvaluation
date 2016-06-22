using System;

namespace Domain
{
    public class Datum<T> : DatumBase
    {
        public static Datum<T> CreateNone(Signal signal, DateTime timestamp)
        {
            return new Datum<T>()
            {
                Id = 0,
                Signal = signal,
                Timestamp = timestamp,
                Value = default(T),
                Quality = Quality.None
            };
        }

        public virtual T Value { get; set; }

        [Infrastructure.NHibernateIgnore]
        public override object AbstractValue {  get { return this.Value; } }

        [Infrastructure.NHibernateIgnore]
        public override Type NativeDataType {  get { return typeof(T); } }
    }
}
