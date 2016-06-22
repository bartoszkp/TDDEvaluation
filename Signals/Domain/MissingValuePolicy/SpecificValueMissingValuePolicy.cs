using Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public SpecificValueMissingValuePolicy()
        {
            this.Value = default(T);
            this.Quality = Quality.None;
        }

        public SpecificValueMissingValuePolicy(T value, Quality quality)
        {
            this.Value = value;
            this.Quality = quality;
        }

        protected override IEnumerable<DatumBase> FillMissingData(TimeEnumerator timeEnumerator, IEnumerable<DatumBase> readData)
        {
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : new Datum<T>() { Value = Value, Quality = Quality, Signal = Signal, Timestamp = ts })
                .ToArray();
        }
    }
}
