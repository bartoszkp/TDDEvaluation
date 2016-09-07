using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Repositories;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {

        private Quality Quality = Quality.None;
        private T Value = default(T);
 
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, ISignalsDataRepository signalsDataRepository)
        {
            returnListDatum = new List<Datum<T>>();
            if((data.Last(x=>x==x).Timestamp)<fromIncludedUtc)
            {
                var dataOlder = data.Last(x => x == x);
                this.Quality = dataOlder.Quality;
                this.Value = dataOlder.Value;
            }
            while (fromIncludedUtc < toExcludedUtc)
            {
                Datum<T> elementOfList = data.FirstOrDefault(x => x.Timestamp == fromIncludedUtc);
                if (elementOfList != null)
                {
                    this.Quality = elementOfList.Quality;
                    this.Value = elementOfList.Value;

                }
                returnListDatum.Add(new Datum<T>() { Signal = signal, Quality = this.Quality, Timestamp = fromIncludedUtc, Value = this.Value });
                fromIncludedUtc = AddToDateTime(fromIncludedUtc, signal);
            }
            return returnListDatum;
        }

        public static DateTime AddToDateTime(DateTime date, Signal signal)
        {
            var addTimeSpan = new Dictionary<Granularity, Action>
                {
                    {Granularity.Day,() => date = date.AddDays(1)},
                    {Granularity.Hour,() => date = date.AddHours(1)},
                    {Granularity.Minute,() => date = date.AddMinutes(1)},
                    {Granularity.Month,() => date = date.AddMonths(1)},
                    {Granularity.Second,() => date = date.AddSeconds(1)},
                    {Granularity.Week,() => date = date.AddDays(7)},
                    {Granularity.Year,() => date = date.AddYears(1)}
                };
            addTimeSpan[signal.Granularity].Invoke();
            return date;
        }

        private List<Datum<T>> returnListDatum;

    }
}
