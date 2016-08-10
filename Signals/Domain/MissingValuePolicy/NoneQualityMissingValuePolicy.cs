using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using System;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual IEnumerable<Domain.Datum<T>> SetMissingValue(IEnumerable<Domain.Datum<T>> datums)
        {
            List<Domain.Datum<T>> datumsList = datums.ToList();

            Domain.Signal signal = datumsList.First()?.Signal;

            Domain.Granularity granularity = signal.Granularity;

            DateTime timeStampMin = datumsList.Min(d => d.Timestamp);
            DateTime timeStampMax = datumsList.Max(d => d.Timestamp);

            DateTime currentTimeSamp = timeStampMin;
            while (currentTimeSamp != timeStampMax)
            {
                currentTimeSamp = AddGranular(currentTimeSamp, granularity);
                var currentDatum = (from x in datumsList
                                    where x.Timestamp == currentTimeSamp
                                    select x).FirstOrDefault();

                if (currentDatum == null)
                {
                    Domain.Datum<T> missingDatum = new Domain.Datum<T>()
                    {
                        Quality = Domain.Quality.None,
                        Value = default(T),
                        Timestamp = currentTimeSamp,
                    };
                    datumsList.Add(missingDatum);
                }
            }

            return datumsList;
        }
        private DateTime AddGranular(DateTime timeStamp, Domain.Granularity granularity)
        {
            switch (granularity)
            {
                case Domain.Granularity.Second: { return timeStamp.AddSeconds(1); }
                case Domain.Granularity.Minute: { return timeStamp.AddMinutes(1); }
                case Domain.Granularity.Hour: { return timeStamp.AddHours(1); }
                case Domain.Granularity.Day: { return timeStamp.AddDays(1); }
                case Domain.Granularity.Week: { return timeStamp.AddDays(7); }
                case Domain.Granularity.Month: { return timeStamp.AddMonths(1); }
                case Domain.Granularity.Year: { return timeStamp.AddYears(1); }
                default: return default(DateTime);
            }
        }
    }
}
