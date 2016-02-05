using System;

namespace Domain
{
    public class Datum<T>
    {
        public virtual int Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }
    }
}
