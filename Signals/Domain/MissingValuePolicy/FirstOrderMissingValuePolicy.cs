using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
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

            List<Datum<T>> filledList = new List<Datum<T>>();
            var datumsArray = datums.ToArray();

            if (datumsArray.Length == 0)
                EmptyDatumsCase(signal, filledList, fromIncludedUtc, toExcludedUtc, earlierDatum, laterDatum);
            else
                RegularCase(signal, filledList, datumsArray, fromIncludedUtc, toExcludedUtc, earlierDatum, laterDatum);            

            return filledList;            
        }

        private void EmptyDatumsCase(Signal signal, List<Datum<T>> filledList, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> earlierDatum, Datum<T> laterDatum)
        {
            if(earlierDatum == null || laterDatum == null)
            {
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
                if (tmp >= fromIncludedUtc && tmp < toExcludedUtc)
                    filledList.Add(Datum<T>.CreateNone(signal, tmp));

                tmp = DateHelper.NextDate(tmp, signal.Granularity);
                ++count;
            }

            switch(signal.DataType)
            {
                case DataType.Decimal:
                    FillDatums(filledList as List<Datum<decimal>>, earlierDatum as Datum<decimal>, laterDatum as Datum<decimal>, count, firstMultiplier);
                    break;
                case DataType.Double:
                    FillDatums(filledList as List<Datum<double>>, earlierDatum as Datum<double>, laterDatum as Datum<double>, count, firstMultiplier);
                    break;
                case DataType.Integer:
                    FillDatums(filledList as List<Datum<int>>, earlierDatum as Datum<int>, laterDatum as Datum<int>, count, firstMultiplier);
                    break;
                default:
                    throw new Exception();
            }
            
        }

        private void FillDatums(List<Datum<decimal>> filledList, Datum<decimal> earlierDatum, Datum<decimal> laterDatum, int count, int firstMultiplier)
        {
            var firstVal = earlierDatum.Value;
            var lastVal = laterDatum.Value;
            var delta = (lastVal - firstVal)/ count;

            for (int i = 0; i < filledList.Count; ++i)
            {
                filledList[i].Value = firstVal + delta * (firstMultiplier + i);
                filledList[i].Quality = laterDatum.Quality;
            }
        }

        private void FillDatums(List<Datum<double>> filledList, Datum<double> earlierDatum, Datum<double> laterDatum, int count, int firstMultiplier)
        {
            var firstVal = earlierDatum.Value;
            var lastVal = laterDatum.Value;
            var delta = (firstVal + lastVal) / count;

            for (int i = 0; i < filledList.Count; ++i)
            {
                filledList[i].Value = firstVal + delta * (firstMultiplier + i);
                filledList[i].Quality = laterDatum.Quality;
            }
        }

        private void FillDatums(List<Datum<int>> filledList, Datum<int> earlierDatum, Datum<int> laterDatum, int count, int firstMultiplier)
        {
            var firstVal = earlierDatum.Value;
            var lastVal = laterDatum.Value;
            var delta = (firstVal + lastVal) / count;

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

        private void RunInterpolation<T>(DataType datatype, List<Datum<T>> filledList, ref int interFirst, ref int interLast, ref bool interpolating)
        {
            if(datatype == DataType.Decimal)
                InterpolateValues(filledList as List<Datum<decimal>>, interFirst, interLast);
            else if (datatype == DataType.Double)
                InterpolateValues(filledList as List<Datum<double>>, interFirst, interLast);
            else if (datatype == DataType.Integer)
                InterpolateValues(filledList as List<Datum<int>>, interFirst, interLast);

            interpolating = false;
            interFirst = -1;
            interLast = -1;
        }

        private void InterpolateValues(List<Datum<double>> filledList, int first, int last)
        {
            if (first <= 0 || last > filledList.Count - 1)
                throw new ArgumentException("Trying to interpolate values for wrong indexes.");
                        
            var count = (last - first) + 2;
            var firstVal = filledList[first - 1].Value;
            var lastVal = filledList[last + 1].Value;
            var quality = filledList[last + 1].Quality;
            var delta = (lastVal-firstVal)/count;

            int i = first;
            int j = 1;
            while(i <= last)
            {
                filledList[i].Value = firstVal + delta*j;
                filledList[i].Quality = quality;
                ++i;++j;
            }
        }

        private void InterpolateValues(List<Datum<decimal>> filledList, int first, int last)
        {
            if (first <= 0 || last > filledList.Count - 1)
                throw new ArgumentException("Trying to interpolate values for wrong indexes.");

            var count = (last - first) + 2;
            var firstVal = filledList[first - 1].Value;
            var lastVal = filledList[last + 1].Value;
            var quality = filledList[last + 1].Quality;
            var delta = (lastVal - firstVal) / count;

            int i = first;
            int j = 1;
            while (i <= last)
            {
                filledList[i].Value = firstVal + delta * j;
                filledList[i].Quality = quality;
                ++i; ++j;
            }
        }

        private void InterpolateValues(List<Datum<int>> filledList, int first, int last)
        {
            if (first <= 0 || last > filledList.Count - 1)
                throw new ArgumentException("Trying to interpolate values for wrong indexes.");

            var count = (last - first) + 2;
            var firstVal = filledList[first - 1].Value;
            var lastVal = filledList[last + 1].Value;
            var quality = filledList[last + 1].Quality;
            var delta = (lastVal - firstVal) / count;

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
