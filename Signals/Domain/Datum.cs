using System;

namespace Domain
{
    [Infrastructure.Component]
    public class Datum<T>
    {
        public virtual Signal Signal { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual T Value { get; set; }
    }
}
