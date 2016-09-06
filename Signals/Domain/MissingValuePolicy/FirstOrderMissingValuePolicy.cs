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

            List<Datum<T>> filledList = new List<Datum<T>>();
            var datumsArray = datums.ToList();

            if (fromIncludedUtc == toExcludedUtc && datumsArray.Count == 1)
            {
                filledList.Add(datumsArray[0]);
                return filledList;
            }

            if (signal.DataType == DataType.Boolean || signal.DataType == DataType.String)
                throw new Exception("First Order MVP can be set only for numeric types.");

            /////////////////////////////////////////////////////////////////////////////
            int counter = 1;
            var tmpDate = fromIncludedUtc;
            while(tmpDate < toExcludedUtc)
            {
                var earlier = earlierDatum;
                var later = laterDatum;
                var tmpDatum = datumsArray.FirstOrDefault(s => s.Timestamp == tmpDate);

                if (tmpDatum == null)
                {
                    if (earlier == null || later == null)
                    {
                        filledList.Add(Datum<T>.CreateNone(signal, tmpDate));
                    }

                    else
                    {
                        var sampleQuantity = SampleCount(signal, earlierDatum.Timestamp, laterDatum.Timestamp);

                        filledList.Add(new Datum<T>()
                        {
                            Timestamp = tmpDate,
                            Signal = signal,
                            Value = ValueSample(signal, earlier, later, sampleQuantity, counter),
                            Quality = WorestQuality(earlier.Quality, later.Quality)
                        });
                    }

                }

                else filledList.Add(tmpDatum);

                counter++;
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
                        var later = Convert.ToDecimal(laterDatum);
                        var sample = Math.Round(((Math.Abs(later - earlier) / sampleCount)*counter)+ earlier, 4);
                        return (T)sample.Adapt(typeof(decimal),typeof(decimal));
                    }
                case DataType.Double:
                    {
                        var earlier = Convert.ToDouble(earlierDatum.Value);
                        var later = Convert.ToDouble(laterDatum);
                        var sample = Math.Round(((Math.Abs(later - earlier) / sampleCount) * counter) + earlier, 4);
                        return (T)sample.Adapt(typeof(double), typeof(double));
                    }
                case DataType.Integer:
                    {
                        var earlier = Convert.ToInt32(earlierDatum.Value);
                        var later = Convert.ToInt32(laterDatum);
                        var sample = Convert.ToInt32(((Math.Abs(later - earlier) / sampleCount) * counter) + earlier);
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

        private void EmptyDatumsCase(Signal signal, List<Datum<T>> filledList, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> earlierDatum, Datum<T> laterDatum)
        {
            if(earlierDatum == null || laterDatum == null)
            {
                if (fromIncludedUtc == toExcludedUtc)
                {
                    filledList.Add(Datum<T>.CreateNone(signal, fromIncludedUtc));
                    return;
                }
                                    
                var stamp = fromIncludedUtc;
                while(stamp < toExcludedUtc)
                {
                    filledList.Add(Datum<T>.CreateNone(signal, stamp));
                    stamp = DateHelper.NextDate(stamp, signal.Granularity);
                }   
                return;
            }

            InterpolateValuesForEmpty(filledList, signal, earlierDatum, laterDatum, fromIncludedUtc, toExcludedUtc);
        }        

        private void InterpolateValuesForEmpty(List<Datum<T>> filledList, Signal signal, Datum<T> earlierDatum, Datum<T> laterDatum, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            int count = 0;
            int firstMultiplier = 0;
            var tmp = earlierDatum.Timestamp;
            while (tmp < laterDatum.Timestamp)
            {
                if (tmp == fromIncludedUtc)
                    firstMultiplier = count;
                if ((tmp >= fromIncludedUtc && tmp < toExcludedUtc) || (fromIncludedUtc == toExcludedUtc && tmp == fromIncludedUtc))
                    filledList.Add(Datum<T>.CreateNone(signal, tmp));

                tmp = DateHelper.NextDate(tmp, signal.Granularity);
                ++count;
            }
                        
            FillDatums(filledList, earlierDatum, laterDatum, count, firstMultiplier);            
        }

        private void FillDatums(List<Datum<T>> filledList, Datum<T> earlierDatum, Datum<T> laterDatum, int count, int firstMultiplier)
        {
            dynamic firstVal = earlierDatum.Value;
            dynamic lastVal = laterDatum.Value;
            dynamic delta = (lastVal - firstVal) / count;

            for (int i = 0; i < filledList.Count; ++i)
            {
                filledList[i].Value = firstVal + delta * (firstMultiplier + i);
                filledList[i].Quality = laterDatum.Quality;
            }
        }

        private void RegularCase(Signal signal, List<Datum<T>> filledList, Datum<T>[] datumsArray, DateTime tmp, DateTime toExcludedUtc, Datum<T> earlierDatum, Datum<T> laterDatum)
        {
            int datumsIter = 0;
            int interFirst = -1;
            int interLast = -1;
            int it = 0;
            bool interpolating = false;

            while (tmp < toExcludedUtc)
            {
                bool fillingOutEarlier = (datumsArray.First().Timestamp >= tmp && datumsIter == 0);
                bool fillingOutLater = (datumsArray.Last().Timestamp <= tmp && datumsIter == datumsArray.Length);

                if (fillingOutEarlier || fillingOutLater)
                {
                    if (datumsArray.First().Timestamp == tmp || datumsArray.Last().Timestamp == tmp)
                    {
                        filledList.Add(datumsArray[datumsIter++]);
                        if (interpolating)
                            RunInterpolation(signal.DataType, filledList, ref interFirst, ref interLast, ref interpolating);
                    }
                    else
                    {
                        if ((fillingOutEarlier && earlierDatum != null) || (fillingOutLater && laterDatum != null))
                            SetInterpolationParameters(ref interpolating, ref interFirst, ref interLast, it);

                        filledList.Add(Datum<T>.CreateNone(signal, tmp));
                    }
                }
                else
                {
                    if (datumsIter < datumsArray.Length && datumsArray[datumsIter].Timestamp == tmp)
                    {
                        filledList.Add(datumsArray[datumsIter++]);
                        if (interpolating)
                            RunInterpolation(signal.DataType, filledList, ref interFirst, ref interLast, ref interpolating);
                    }
                    else
                    {
                        SetInterpolationParameters(ref interpolating, ref interFirst, ref interLast, it);
                        filledList.Add(Datum<T>.CreateNone(signal, tmp));
                    }
                }

                ++it;
                tmp = DateHelper.NextDate(tmp, signal.Granularity);
            }
        }

        private void SetInterpolationParameters(ref bool interpolating, ref int interFirst, ref int interLast, int index)
        {
            if (!interpolating)
            {
                interpolating = true;
                interFirst = index;
                interLast = index;
            }
            else
                interLast++;
        }

        private void RunInterpolation(DataType datatype, List<Datum<T>> filledList, ref int interFirst, ref int interLast, ref bool interpolating)
        {
            InterpolateValues(filledList, interFirst, interLast);
            interpolating = false;
            interFirst = -1;
            interLast = -1;
        }

        private void InterpolateValues(List<Datum<T>> filledList, int first, int last)
        {
            if (first <= 0 || last > filledList.Count - 1)
                throw new ArgumentException("Trying to interpolate values for wrong indexes.");

            var count = (last - first) + 2;
            dynamic firstVal = filledList[first - 1].Value;
            dynamic lastVal = filledList[last + 1].Value;
            var quality = filledList[last + 1].Quality;
            dynamic delta = (lastVal - firstVal) / count;

            int i = first;
            int j = 1;
            while (i <= last)
            {
                filledList[i].Value = firstVal + delta * j;
                filledList[i].Quality = quality;
                ++i; ++j;
            }
        }
        
    }
}
