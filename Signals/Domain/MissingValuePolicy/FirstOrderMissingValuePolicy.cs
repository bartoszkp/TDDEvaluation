using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Mapster;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> SetMissingValue(Signal signal, IEnumerable<Datum<T>> datums, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> earlierDatum , Datum<T> laterDatum)
        {
            if (fromIncludedUtc > toExcludedUtc)
                return new List<Datum<T>>();

            if (signal.DataType == DataType.Boolean || signal.DataType == DataType.String)
                throw new Exception("First Order MVP can be set only for numeric types.");

            List<Datum<T>> filledList = new List<Datum<T>>();
            var datumsArray = datums.ToList();

            var tmpDate = fromIncludedUtc;

            if (fromIncludedUtc == toExcludedUtc)
            {
                var earlier = earlierDatum;
                var later = datumsArray.FirstOrDefault(s => s.Timestamp > tmpDate);
                var tmpDatum = datumsArray.FirstOrDefault(s => s.Timestamp == tmpDate);

                if (tmpDatum == null)
                {
                    if (earlier == null || later == null)
                    {
                        if (earlier == null) earlier = datumsArray.LastOrDefault(s => s.Timestamp < tmpDate);
                        if (later == null) later = laterDatum;

                        if (earlier == null || later == null) filledList.Add(Datum<T>.CreateNone(signal, tmpDate));

                        else
                        {
                            var sampleSmallQuantity = SampleCount(signal, earlier.Timestamp, tmpDate);
                            var sampleQuantity = SampleCount(signal, earlier.Timestamp, later.Timestamp);

                            filledList.Add(new Datum<T>()
                            {
                                Timestamp = tmpDate,
                                Signal = signal,
                                Value = ValueSample(signal, earlier, later, sampleQuantity, sampleSmallQuantity),
                                Quality = WorestQuality(earlier.Quality, later.Quality)
                            });
                        }
                    }
                }

                else filledList.Add(tmpDatum);
                tmpDate = IncreaseDate(signal, tmpDate);
            }

            else
            {
                while (tmpDate < toExcludedUtc)
                {
                    var earlier = earlierDatum;
                    var later = datumsArray.FirstOrDefault(s => s.Timestamp > tmpDate);
                    var tmpDatum = datumsArray.FirstOrDefault(s => s.Timestamp == tmpDate);

                    if (tmpDatum == null)
                    {
                        if (earlier == null || later == null)
                        {
                            if (earlier == null) earlier = datumsArray.LastOrDefault(s => s.Timestamp < tmpDate);
                            if (later == null) later = laterDatum;

                            if (earlier == null || later == null) filledList.Add(Datum<T>.CreateNone(signal, tmpDate));

                            else
                            {
                                var sampleSmallQuantity = SampleCount(signal, earlier.Timestamp, tmpDate);
                                var sampleQuantity = SampleCount(signal, earlier.Timestamp, later.Timestamp);

                                filledList.Add(new Datum<T>()
                                {
                                    Timestamp = tmpDate,
                                    Signal = signal,
                                    Value = ValueSample(signal, earlier, later, sampleQuantity, sampleSmallQuantity),
                                    Quality = WorestQuality(earlier.Quality, later.Quality)
                                });
                            }
                        }
                    }

                    else filledList.Add(tmpDatum);
                    
                    tmpDate = IncreaseDate(signal, tmpDate);
                }
            }
                  

            return filledList;            
        }

        private Quality WorestQuality(Quality q1, Quality q2)
        {
            if (q1 > q2) return q2;
            else return q1;
        }

        private T ValueSample(Signal signal, Datum<T> earlierDatum, Datum<T> laterDatum, int sampleCount, int counter)
        {
            switch(signal.DataType)
            {
                case DataType.Decimal:
                    {
                        var earlier = Convert.ToDecimal(earlierDatum.Value);
                        var later = Convert.ToDecimal(laterDatum.Value);
                        var sample = Math.Round(((Math.Abs(later - earlier) / sampleCount) * counter + earlier), 4);
                        return (T)sample.Adapt(typeof(decimal),typeof(decimal));
                    }
                case DataType.Double:
                    {
                        var earlier = Convert.ToDouble(earlierDatum.Value);
                        var later = Convert.ToDouble(laterDatum.Value);
                        var sample = Math.Round(((Math.Abs(later - earlier) / sampleCount) * counter + earlier), 4);
                        return (T)sample.Adapt(typeof(double), typeof(double));
                    }
                case DataType.Integer:
                    {
                        var earlier = Convert.ToInt32(earlierDatum.Value);
                        var later = Convert.ToInt32(laterDatum.Value);
                        var sample = Convert.ToInt32(((Math.Abs(later - earlier) / sampleCount) * counter + earlier));
                        return (T)sample.Adapt(typeof(int), typeof(int));
                    }
                default: throw new Exception("Datum dataType unsupported");
            }
        }

        private int SampleCount(Signal signal, DateTime earlier, DateTime later)
        {
            int count = 0;
            DateTime tmpDate = earlier;

            while(tmpDate < later)
            {
                tmpDate = IncreaseDate(signal, tmpDate);
                count++;
            }

            return count;
        }

        private DateTime IncreaseDate(Signal signal, DateTime datetime)
        {
            DateTime tmp = new DateTime();
            switch(signal.Granularity)
            {
                case Granularity.Day: return tmp = datetime.AddDays(1);
                case Granularity.Hour: return tmp = datetime.AddHours(1);
                case Granularity.Minute: return tmp = datetime.AddMinutes(1);
                case Granularity.Month: return tmp = datetime.AddMonths(1);
                case Granularity.Second: return tmp = datetime.AddSeconds(1);
                case Granularity.Week: return tmp = datetime.AddDays(7);
                case Granularity.Year: return tmp = datetime.AddYears(1);
                default: return tmp;
            }
        }
    }
}
