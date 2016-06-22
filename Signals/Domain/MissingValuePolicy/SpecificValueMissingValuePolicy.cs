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

        public override IEnumerable<Datum<S>> FillMissingData<S>(TimeEnumerator timeEnumerator, IEnumerable<Datum<S>> readData)
        {
            if (typeof(S) != typeof(T)) // TODO ugly (necessary?)
                throw new InvalidOperationException("SpecificValueMissingValuePolicy used for wrong DataType");

            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            // TODO ugly convert...
            var valueInS = (S)Convert.ChangeType(Value, typeof(S));

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : new Datum<S>() { Value = valueInS, Quality = Quality, Signal = Signal, Timestamp = ts })
                .ToArray();
        }
    }
}
