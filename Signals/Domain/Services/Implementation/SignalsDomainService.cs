using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;

namespace Domain.Services.Implementation
{
    [UnityRegister]
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalsRepository signalsRepository;
        private readonly ISignalsDataRepository signalsDataRepository;
        private readonly IMissingValuePolicyRepository missingValuePolicyRepository;

        public SignalsDomainService(
            ISignalsRepository signalsRepository,
            ISignalsDataRepository signalsDataRepository,
            IMissingValuePolicyRepository missingValuePolicyRepository)
        {
            this.signalsRepository = signalsRepository;
            this.signalsDataRepository = signalsDataRepository;
            this.missingValuePolicyRepository = missingValuePolicyRepository;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)            
                throw new IdNotNullException();            

            var res = this.signalsRepository.Add(newSignal);
            
            if (newSignal.DataType == DataType.Boolean) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<bool>());
            else if (newSignal.DataType == DataType.Decimal) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<decimal>());
            else if (newSignal.DataType == DataType.Double) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<double>());
            else if (newSignal.DataType == DataType.Integer) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<int>());
            else if (newSignal.DataType == DataType.String) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<string>());

            return res;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public IEnumerable<Signal> GetPathEntry(Path prefix)
        {
            IEnumerable<Signal> result = signalsRepository.GetAllWithPathPrefix(prefix);
            return result;
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase policy)
        {
            if (policy != null)
            {
                if (policy.Id.HasValue)
                    throw new IdNotNullException();
                if (policy.NativeDataType != signal.DataType.GetNativeType())
                    throw new TypeMismatchException();
                policy.Signal = signal;
            }
            
            this.missingValuePolicyRepository.Set(signal, policy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var result = this.missingValuePolicyRepository.Get(signal);
            if (result != null)
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;
            else
                return null;
        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);

            SetMissingValuePolicy(signal, null);
            DeleteData(signal);

            signalsRepository.Delete(signal);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            bool areDataTimestampsCorrect = CheckCorrectnessOfDataTimestamps(signal.Granularity, data);

            if (!areDataTimestampsCorrect) throw new ArgumentException();

            data = data.Select(d =>
            {
                d.Signal = signal;
                return d;
            }).ToList();

            this.signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            bool isFromIncludedUtcCorrect = CheckCorrectnessOfDate(signal.Granularity, fromIncludedUtc);

            if (!isFromIncludedUtcCorrect) throw new ArgumentException();

            var data = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(datum => datum.Timestamp);
            var mvp = GetMissingValuePolicy(signal);

            IEnumerable<Datum<T>> filledData = data;

            if (mvp != null)
                filledData = FillMissingRecords(signal, data, fromIncludedUtc, toExcludedUtc, mvp as MissingValuePolicy<T>);

            return filledData;
        }        

        public IEnumerable<Datum<T>> FillMissingRecords<T>(Signal signal, IEnumerable<Datum<T>> data, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc, MissingValuePolicy<T> mvp)
        {
            var dateTimeStep = GetTimeStepFunction(signal.Granularity);

            Datum<T>[] shadowDatums = null;
            if(mvp is ShadowMissingValuePolicy<T>)
            {
                var shadowMVP = mvp as ShadowMissingValuePolicy<T>;
                shadowDatums = signalsDataRepository.GetData<T>(shadowMVP.ShadowSignal, fromIncludedUtc, toExcludedUtc).ToArray();
            }

            if (fromIncludedUtc == toExcludedUtc)
                return new[] { data.FirstOrDefault(datum => datum.Timestamp == fromIncludedUtc)
                                ?? FillMissingRecord(data, signal, fromIncludedUtc, mvp, shadowDatums) };

            List<Datum<T>> list = new List<Datum<T>>();            

            for (DateTime d = fromIncludedUtc; d < toExcludedUtc; d = dateTimeStep(d))
                list.Add(
                    data.FirstOrDefault(datum => datum.Timestamp == d)
                    ?? FillMissingRecord(data, signal, d, mvp, shadowDatums));

            return list;
        }
        private Datum<T> FillMissingRecord<T>(IEnumerable<Datum<T>> data, Signal signal, DateTime dateTime, 
            MissingValuePolicy<T> mvp, Datum<T>[] shadowDatums = null)
        {
            if (mvp is NoneQualityMissingValuePolicy<T> || mvp is SpecificValueMissingValuePolicy<T>)
                return mvp.GetMissingValue(signal, dateTime);            
            else if (mvp is ZeroOrderMissingValuePolicy<T>)
            {
                var previous = data.LastOrDefault(datum => datum.Timestamp < dateTime)
                    ?? signalsDataRepository.GetDataOlderThan<T>(signal, dateTime, 1).SingleOrDefault();

                return mvp.GetMissingValue(signal, dateTime, previous);
            }
            else if (mvp is FirstOrderMissingValuePolicy<T>)
            {
                if (typeof(T) == typeof(string) || typeof(T) == typeof(bool))
                    throw new InvalidPolicyDataTypeException(typeof(T));

                var previous = data.LastOrDefault(datum => datum.Timestamp < dateTime)
                    ?? signalsDataRepository.GetDataOlderThan<T>(signal, dateTime, 1).SingleOrDefault();
                var next = data.FirstOrDefault(datum => datum.Timestamp > dateTime)
                    ?? signalsDataRepository.GetDataNewerThan<T>(signal, dateTime, 1).SingleOrDefault();

                return mvp.GetMissingValue(signal, dateTime, previous, next);
            }
            else if (mvp is ShadowMissingValuePolicy<T>)
            {
                var shadowDatum = shadowDatums.FirstOrDefault(datum => datum.Timestamp == dateTime);
                return mvp.GetMissingValue(signal, dateTime, null, null, shadowDatum);
            }
            else return Datum<T>.CreateNone(signal, dateTime);
        }

        private void DeleteData(Signal signal)
        {
            var type = signal.DataType.GetNativeType();
            var methodInfo = typeof(ISignalsDataRepository).GetMethod("DeleteData").MakeGenericMethod(type);
            methodInfo.Invoke(signalsDataRepository, new[] { signal });
        }

        private bool CheckCorrectnessOfDate(Granularity granularity, DateTime fromIncludedUtc)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    if (fromIncludedUtc.Millisecond != 0)
                        return false;
                    break;
                case Granularity.Minute:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0)
                        return false;
                    break;
                case Granularity.Hour:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0)
                        return false;
                    break;
                case Granularity.Day:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0)
                        return false;
                    break;
                case Granularity.Week:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0 |
                        fromIncludedUtc.DayOfWeek != DayOfWeek.Monday)
                        return false;
                    break;
                case Granularity.Month:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0 |
                        fromIncludedUtc.Day != 1)
                        return false;
                    break;
                case Granularity.Year:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0 |
                        fromIncludedUtc.Day != 1 |
                        fromIncludedUtc.Month != 1)
                        return false;
                    break;
                default:
                    break;
            }

            return true;
        }

        private bool CheckCorrectnessOfDataTimestamps<T>(Granularity granularity, IEnumerable<Datum<T>> data)
        {
            foreach (var datum in data)
                if (!CheckCorrectnessOfDate(granularity, datum.Timestamp))
                    return false;

            return true;
        }

        public static Func<DateTime, DateTime> GetTimeStepFunction(Granularity granularity)
        {
            switch(granularity)
            {
                case Granularity.Day:
                    return d => d.AddDays(1);
                case Granularity.Hour:
                    return d => d.AddHours(1);
                case Granularity.Minute:
                    return d => d.AddMinutes(1);
                case Granularity.Month:
                    return d => d.AddMonths(1);
                case Granularity.Second:
                    return d => d.AddSeconds(1);
                case Granularity.Week:
                    return d => d.AddDays(7);
                case Granularity.Year:
                    return d => d.AddYears(1);
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (signal.Granularity > granularity) throw new ArgumentException();
            if (!CheckCorrectnessOfDate(granularity, fromIncludedUtc) || !CheckCorrectnessOfDate(granularity, toExcludedUtc)) throw new ArgumentException();
            if (!CheckCorrectnessOfDate(signal.Granularity, fromIncludedUtc) || !CheckCorrectnessOfDate(signal.Granularity, toExcludedUtc)) throw new ArgumentException();

            var signalData = GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            var coarseData = GenerateCoarseData<T>(signalData, granularity, GetDateDiff(granularity, fromIncludedUtc, toExcludedUtc));

            return coarseData;
        }

        private int GetDateDiff(Granularity granularity, DateTime fromIncludedUtc, DateTime toExludedUtc)
        {
            switch(granularity)
            {
                case Granularity.Day: return Convert.ToInt32((toExludedUtc - fromIncludedUtc).TotalDays);
                case Granularity.Hour: return Convert.ToInt32((toExludedUtc - fromIncludedUtc).TotalHours);
                case Granularity.Minute: return Convert.ToInt32((toExludedUtc - fromIncludedUtc).TotalMinutes);
                case Granularity.Month: return ((toExludedUtc.Year - fromIncludedUtc.Year) * 12) + toExludedUtc.Month - fromIncludedUtc.Month;
                case Granularity.Second: return Convert.ToInt32((toExludedUtc - fromIncludedUtc).TotalSeconds);
                case Granularity.Week: return Convert.ToInt32((toExludedUtc - fromIncludedUtc).TotalDays/7);
                case Granularity.Year: return toExludedUtc.Year - fromIncludedUtc.Year;
                default: throw new NotImplementedException();
            }
        }

        private IEnumerable<Datum<T>> GenerateCoarseData<T>(IEnumerable<Datum<T>> signalData, Granularity granularity, int dataSets)
        {
            List<Datum<T>> result = new List<Datum<T>>();
            var dataSetsElementQuantity = signalData.Count() / dataSets;
            var countQuantity = signalData.Count() / dataSets;
            var counter = 0;

            for(int i = counter; i < signalData.Count(); i += countQuantity)
            {
                Quality quality = new Quality();
                quality = signalData.ElementAt(i).Quality;

                for (int j = counter; j < dataSetsElementQuantity; j++)
                {
                    if (quality == Quality.None) break;
                    if (signalData.ElementAt(j).Quality == Quality.None) { quality = Quality.None; break; }
                    else
                    {
                        if (signalData.ElementAt(j).Quality > quality) quality = signalData.ElementAt(j).Quality;
                    }
                }

                result.Add(new Datum<T>() { Timestamp = signalData.ElementAt(counter).Timestamp, Quality = quality, Value = SumValue<T>(counter, dataSetsElementQuantity, signalData) });
                counter = dataSetsElementQuantity;
                dataSetsElementQuantity += countQuantity;
            }

            return result;
        }



        private T SumValue<T>(int i, int j, IEnumerable<Datum<T>> signalData)
        {
            if (signalData.First().Value is bool) throw new NotImplementedException("This function doesnt handle bool type values");
            if (signalData.First().Value is string) throw new NotImplementedException("This function doesnt handle string type values");
            if (signalData.First().Value is decimal)
            {
                List<Datum<decimal>> tmpList = new List<Datum<decimal>>();
                signalData.ToList().ForEach(d => { tmpList.Add(new Datum<decimal>() { Value = Convert.ToDecimal(d.Value) }); });
                decimal tmpAverage = 0;
                int count = 0;

                for (int k = i; k < j; k++)
                {
                    tmpAverage = +tmpList[k].Value;
                    count++;
                }

                tmpAverage = tmpAverage / count;
                return (T)tmpAverage.Adapt(typeof(decimal), typeof(T));
            }

            if (signalData.First().Value is double)
            {
                List<Datum<double>> tmpList = new List<Datum<double>>();
                signalData.ToList().ForEach(d => { tmpList.Add(new Datum<double>() { Value = Convert.ToDouble(d.Value) }); });
                double tmpAverage = 0;
                int count = 0;

                for (int k = i; k < j; k++)
                {
                    tmpAverage = +tmpList[k].Value;
                    count++;
                }

                tmpAverage = tmpAverage / count;
                return (T)tmpAverage.Adapt(typeof(double), typeof(T));
            }

            if (signalData.First().Value is int)
            {
                List<Datum<int>> tmpList = new List<Datum<int>>();
                signalData.ToList().ForEach(d => { tmpList.Add(new Datum<int>() { Value = Convert.ToInt32(d.Value) }); });
                int tmpAverage = 0;
                int count = 0;

                for (int k = i; k < j; k++)
                {
                    tmpAverage += tmpList[k].Value;
                    count++;
                }

                tmpAverage = tmpAverage / count;
                return (T)tmpAverage.Adapt(typeof(int), typeof(T));
            }

            else throw new NotImplementedException();
        }
    }
}
