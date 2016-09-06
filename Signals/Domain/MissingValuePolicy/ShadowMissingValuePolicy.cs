using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Repositories;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public virtual IEnumerable<Datum<T>> SetMissingValues(ISignalsDataRepository repository, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var myDatums = repository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            var shadowDatums = repository.GetData<T>(ShadowSignal, fromIncludedUtc, toExcludedUtc);

            if(fromIncludedUtc == toExcludedUtc)
                return new[]
                {
                    myDatums.FirstOrDefault(d => d.Timestamp == fromIncludedUtc)
                    ?? shadowDatums.FirstOrDefault(d => d.Timestamp == fromIncludedUtc)
                    ?? Datum<T>.CreateNone(signal, fromIncludedUtc)
                };

            var filledData = new List<Datum<T>>();
            var tmp = fromIncludedUtc;

            while(tmp < toExcludedUtc)
            {
                filledData.Add(myDatums.FirstOrDefault(d => d.Timestamp == tmp)
                    ?? shadowDatums.FirstOrDefault(d => d.Timestamp == tmp)
                    ?? Datum<T>.CreateNone(signal, tmp));

                tmp = AddTimpestamp(signal, tmp);
            }

            return filledData;
        }

        private DateTime AddTimpestamp(Signal signal, DateTime datetime)
        {
            switch (signal.Granularity)
            {
                case Granularity.Day: return datetime.AddDays(1);
                case Granularity.Hour: return datetime.AddHours(1);
                case Granularity.Minute: return datetime.AddMinutes(1);
                case Granularity.Month: return datetime.AddMonths(1);
                case Granularity.Second: return datetime.AddSeconds(1);
                case Granularity.Week: return datetime.AddDays(7);
                case Granularity.Year: return datetime.AddYears(1);
                default: return datetime;
            }
        }
    }
}
