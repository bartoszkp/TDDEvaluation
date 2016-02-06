using System;

namespace Domain
{
    public class Datum<T>
    {
        public static Datum<T> None(Signal signal, DateTime timestamp)
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

        public virtual int Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }
    }
}
