using System;

namespace Domain
{
    public class Datum<T>
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
        public static Datum<T> CreateSpecific<T>(Signal signal, DateTime timestamp, Quality quality, T Value)
        {
            return new Datum<T>()
            {
                Id = 0,
                Signal = signal,
                Timestamp = timestamp,
                Value = Value,
                Quality = quality,
            };
        }

        public virtual int Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }
    }
}
