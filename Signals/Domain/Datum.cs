using System;

namespace Domain
{
    public class Datum<T>
    {
        public virtual DateTime Timestamp { get; set; }

        public virtual T Value { get; set; }
    }
}
