using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> olderDatum, Datum<T> newestDatum)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Datum<T>> FillData(Signal signal, List<Datum<T>> signalData, List<Datum<T>> shadowSignalData, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            while (fromIncludedUtc < toExcludedUtc)
            {
                if (signalData.Find(s => s.Timestamp == fromIncludedUtc) == null)
                {
                    var missingDatum = shadowSignalData.Find(s => s.Timestamp == fromIncludedUtc);

                    if (missingDatum == null)
                    {
                        signalData.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Timestamp = fromIncludedUtc,
                            Value = default(T)
                        });
                    }

                    else
                        signalData.Add(missingDatum);
                    

                }

                fromIncludedUtc = CalculateNextTimeStamp(signal.Granularity, fromIncludedUtc);
                
            }


            return signalData;
        }

        private DateTime CalculateNextTimeStamp(Granularity granularity,DateTime previousTimeStamp)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return previousTimeStamp.AddSeconds(1);

                case Granularity.Minute:
                    return previousTimeStamp.AddMinutes(1);

                case Granularity.Hour:
                    return previousTimeStamp.AddHours(1);

                case Granularity.Day:
                    return previousTimeStamp.AddDays(1);

                case Granularity.Week:
                    return previousTimeStamp.AddDays(7);

                case Granularity.Month:
                    return previousTimeStamp.AddMonths(1);
                    
                case Granularity.Year:
                    return previousTimeStamp.AddYears(1);

                default:
                    throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");
            }
        }

        


    }
}
