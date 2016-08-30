﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> SetMissingValue(Signal signal, IEnumerable<Datum<T>> datums, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> earlierDatum = null, Datum<T> laterDatum = null)
        {
            if (fromIncludedUtc > toExcludedUtc)
                return new List<Datum<T>>();
            else if (fromIncludedUtc == toExcludedUtc)
            {
                var datum = datums.FirstOrDefault(d => d.Timestamp == fromIncludedUtc);
                return new[] { datum ?? Datum<T>.CreateNone(signal, fromIncludedUtc) };
            }
            else
            {
                List<Datum<T>> filledList = new List<Datum<T>>();

                Granularity granularity = signal.Granularity;

                DateTime tmp = fromIncludedUtc;

                while (tmp < toExcludedUtc)
                {
                    AddToTheListSuitableDatum(filledList, tmp, signal, datums, earlierDatum);

                    tmp = DateHelper.NextDate(tmp, granularity);
                }

                return filledList;
            }
        }

        private void AddToTheListSuitableDatum(List<Datum<T>> filledList, DateTime tmp, Signal signal, IEnumerable<Domain.Datum<T>> datums, Datum<T> earlierDatum)
        {
            Datum<T> newDatum = datums.FirstOrDefault(datum => datum.Timestamp == tmp);

            if (newDatum == null)
            {
                var last = filledList.LastOrDefault();
                if (last != null)
                    newDatum = new Datum<T>
                    {
                        Quality = last.Quality,
                        Signal = Signal,
                        Timestamp = tmp,
                        Value = last.Value
                    };
                else
                {
                    if(earlierDatum == null)
                        newDatum = Datum<T>.CreateNone(signal, tmp);
                    else
                    {
                        newDatum = new Datum<T>
                        {
                            Quality = earlierDatum.Quality,
                            Signal = Signal,
                            Timestamp = tmp,
                            Value = earlierDatum.Value
                        };
                    }
                }
                    
            }
            filledList.Add(newDatum);
        }
    }
}
