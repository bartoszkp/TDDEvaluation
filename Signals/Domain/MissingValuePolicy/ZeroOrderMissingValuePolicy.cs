using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual IEnumerable<Datum<T>> SetMissingValues(Repositories.ISignalsDataRepository repository, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (fromIncludedUtc > toExcludedUtc)  return new List<Datum<T>>(); 

            var filledData = new List<Datum<T>>();
            DateTime tmp = fromIncludedUtc;

            if (fromIncludedUtc == toExcludedUtc)
            {
                AddToListSuitableDatum(repository, signal, filledData, tmp); 

                return filledData;
            }

            else
            {
                switch (signal.Granularity)
                {
                    case Granularity.Second:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, filledData, tmp);

                            tmp = tmp.AddSeconds(1);
                        }

                        break;

                    case Granularity.Minute:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, filledData, tmp);

                            tmp = tmp.AddMinutes(1);
                        }

                        break;

                    case Granularity.Hour:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, filledData, tmp);

                            tmp = tmp.AddHours(1);
                        }

                        break;

                    case Granularity.Day:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, filledData, tmp);

                            tmp = tmp.AddDays(1);
                        }

                        break;

                    case Granularity.Week:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, filledData, tmp);

                            tmp = tmp.AddDays(7);
                        }

                        break;

                    case Granularity.Month:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, filledData, tmp);

                            tmp = tmp.AddMonths(1);
                        }

                        break;

                    case Granularity.Year:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, filledData, tmp);

                            tmp = tmp.AddYears(1);
                        }

                        break;

                    default: break;
                }
            }

            return filledData;

        }

        private void AddToListSuitableDatum(Repositories.ISignalsDataRepository repository, Signal signal, List<Datum<T>> filledData, DateTime tmp)
        {
            var previousDatum = repository.GetDataOlderThan<T>(signal, tmp.AddSeconds(1), 1).FirstOrDefault();

            if (previousDatum == null) filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T), Timestamp = tmp });

            else filledData.Add (new Datum<T>() { Quality = previousDatum.Quality, Value = previousDatum.Value, Timestamp = tmp });
        }
    }
}