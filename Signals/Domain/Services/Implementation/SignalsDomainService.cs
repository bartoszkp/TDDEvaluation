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

            newSignal = signalsRepository.Add(newSignal);
            var type = DataTypeUtils.GetNativeType(newSignal.DataType);
            var mvp = typeof(NoneQualityMissingValuePolicy<>).MakeGenericType(type);
            missingValuePolicyRepository.Set(newSignal, (MissingValuePolicyBase)Activator.CreateInstance(mvp));

            return newSignal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public void Delete(int signalId)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
                throw new ArgumentException();

            DeleteDataByDataType(signal);
            missingValuePolicyRepository.Set(signal, null);

            signalsRepository.Delete(signal);
        }

        private void DeleteDataByDataType(Signal signal)
        {
            switch (signal.DataType) {
                case DataType.Boolean:
                    signalsDataRepository.DeleteData<bool>(signal);
                    break;
                case DataType.Integer:
                    signalsDataRepository.DeleteData<int>(signal);
                    break;
                case DataType.Double:
                    signalsDataRepository.DeleteData<double>(signal);
                    break;
                case DataType.Decimal:
                    signalsDataRepository.DeleteData<decimal>(signal);
                    break;
                case DataType.String:
                    signalsDataRepository.DeleteData<string>(signal);
                    break;
                default: throw new NotSupportedException("Signals DataType is currently not supported");
            }
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var res = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(x => x.Timestamp).ToList();
            var olderData = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
            var newerData = signalsDataRepository.GetDataNewerThan<T>(signal, toExcludedUtc, 1);
            var mvp = Get(signal);

            if(signal.DataType == DataType.Boolean || signal.DataType == DataType.String)
            {
                if (mvp.GetType() == typeof(FirstOrderMissingValuePolicy<T>))
                    throw new NotSupportedException();
            }

            if (mvp == null) mvp = new NoneQualityMissingValuePolicy<T>();

            int index = 0;

            if (mvp is ShadowMissingValuePolicy<T>)
            {
                newerData = GetData<T>((mvp as ShadowMissingValuePolicy<T>).ShadowSignal, fromIncludedUtc, toExcludedUtc);
            }

            if (fromIncludedUtc == toExcludedUtc)
            {
                var datum = (from x in res
                             where x.Timestamp == fromIncludedUtc
                             select x).FirstOrDefault();

                if (datum == null)
                {
                    var tempDatums = (mvp as MissingValuePolicy<T>).GetDatum(fromIncludedUtc, signal.Granularity, res, olderData, newerData);
                    foreach (var tempDatum in tempDatums.Where(x => x.Timestamp == fromIncludedUtc))
                    {
                        tempDatum.Signal = signal;
                        res.Insert(index, tempDatum);
                    }
                }
            }

            while (fromIncludedUtc < toExcludedUtc)
            {
                var datum = (from x in res
                             where x.Timestamp == fromIncludedUtc
                             select x).FirstOrDefault();

                if (datum == null)
                {
                    olderData = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
                    var tempDatums = (mvp as MissingValuePolicy<T>).GetDatum(fromIncludedUtc,signal.Granularity, res, olderData, newerData);
                    foreach (var tempDatum in tempDatums.Where(x => x.Timestamp < toExcludedUtc && x.Timestamp >= fromIncludedUtc))
                    {
                        tempDatum.Signal = signal;
                        res.Insert(index, tempDatum);
                    }
                }
                ++index;
                fromIncludedUtc = AddTime(signal.Granularity, fromIncludedUtc);
            }
             
            return res.OrderBy(x => x.Timestamp);
        }

        public void Set<T>(Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            if(missingValuePolicy is ShadowMissingValuePolicy<T>)
            {
                (missingValuePolicy as ShadowMissingValuePolicy<T>).CheckSignalDataTypeAndGranularity(signal);
                CheckIfSignalIsOwnShadow<T>(missingValuePolicy, signal); //throws ArgumentException
            }

            missingValuePolicyRepository?.Set(signal, missingValuePolicy);
        }

        private void CheckIfSignalIsOwnShadow<T>(MissingValuePolicyBase missingValuePolicy, Signal signal)
        {
            if (missingValuePolicy is ShadowMissingValuePolicy<T>)
            {
                var shadowMvp = missingValuePolicy as ShadowMissingValuePolicy<T>;
                var shadowSignal = shadowMvp.ShadowSignal;
                while (true)
                {
                    if (AreSignalsTheSame(signal, shadowSignal))
                        throw new ArgumentException("signal is own shadow");
                    shadowMvp = Get(shadowSignal) as ShadowMissingValuePolicy<T>;
                    if (shadowMvp == null)
                        break;

                    shadowSignal = shadowMvp.ShadowSignal;
                }
            }
        }

        private bool AreSignalsTheSame(Signal signal1, Signal signal2)
        {
            return (signal1.DataType == signal2.DataType)
                && (signal1.Granularity == signal2.Granularity)
                && Enumerable.SequenceEqual(signal1.Path.Components.ToArray(), signal2.Path.Components.ToArray());
        }

        public MissingValuePolicyBase Get(Signal signal)
        {
          var mvp=  missingValuePolicyRepository.Get(signal);
            return (mvp == null) ? null : TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;

        }

        public Signal Get(Path path)
        {
            return signalsRepository.Get(path);
        }

        public static DateTime AddTime(Granularity granuality,DateTime date)
        {
            if (granuality == Granularity.Second) return  date.AddSeconds(1);
            if (granuality == Granularity.Minute) return date.AddMinutes(1);
            if (granuality == Granularity.Hour) return date.AddHours(1);
            if (granuality == Granularity.Day) return date.AddDays(1);
            if (granuality == Granularity.Week) return date.AddDays(7);
            if (granuality == Granularity.Month) return date.AddMonths(1);
            if (granuality == Granularity.Year) return date.AddYears(1);
            return date;
        }

        public PathEntry GetPathEntry(Path path)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(path);
            var pathDeep = path.Components.Count();
            var resultSignals = new List<Signal>();
            var resultSubPaths = new List<Path>();
            
            foreach(var s in signals)
            {
                if (s.Path.ToString() == path.ToString()) continue;

                if (s.Path.Components.Count() > pathDeep + 1)
                    resultSubPaths.Add(GetSubPath(s, pathDeep + 1));
                else
                    resultSignals.Add(s);
            }

            return new PathEntry(resultSignals, resultSubPaths.Distinct());
        }

        private Path GetSubPath(Signal signal, int deep)
        {
            return Path.FromString(string.Join("/", signal.Path.Components.Take(deep).ToArray()));
        }
    }
}
