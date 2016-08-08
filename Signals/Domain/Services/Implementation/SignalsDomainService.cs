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
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);
            var policyInstance = typeof(NoneQualityMissingValuePolicy<>)
                .MakeGenericType(new Type[] { DataTypeUtils.GetNativeType(signal.DataType) });

            SetMissingValuePolicy(signal, Activator.CreateInstance(policyInstance) as MissingValuePolicyBase);

            return signal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path path)
        {
            return signalsRepository.Get(path);
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase domainMissingValuePolicy)
        {
            if (signal == null)
            {
                throw new ArgumentException("no signal with this id");
            }
            missingValuePolicyRepository.Set(signal, domainMissingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            if (signal == null)
            {
                throw new ArgumentException("no signal with this id");
            }
            var mvp = missingValuePolicyRepository.Get(signal);

            if(mvp == null)
            {
                return null;
            }

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(IEnumerable<Datum<T>> domainData)
        {
            signalsDataRepository.SetData<T>(domainData);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var getData = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.OrderBy(s => s.Timestamp).ToArray();
            var getMissingValuePolicy = GetMissingValuePolicy(signal);

            if (getMissingValuePolicy == null)
                return getData;
            
            var result = new List<Datum<T>>();

            for(DateTime i = fromIncludedUtc; i < toExcludedUtc; i = GetNextDate(i, signal.Granularity))
            {
                var datum = getData.FirstOrDefault(d => CompareDate(d.Timestamp, i, signal.Granularity));

                if (datum == null)
                    datum = GenerateDatumFromPolicy<T>(getMissingValuePolicy as MissingValuePolicy<T>, signal, i);

                result.Add(datum);
            }
            return result;
        }

        private Datum<T> GenerateDatumFromPolicy<T>(MissingValuePolicy<T> mvp, Signal signal, DateTime timestamp)
        {
            var result = new Datum<T>()
            {
                Signal = signal,
                Timestamp = timestamp
            };

            if(mvp is NoneQualityMissingValuePolicy<T>)
            {
                result.Quality = Quality.None;
                result.Value = default(T);
            }

            return result;
        }

        private DateTime GetNextDate(DateTime dt, Granularity granularity)
        {
            if (granularity == Granularity.Year)
                return dt.AddYears(1);
            else if (granularity == Granularity.Month)
                return dt.AddMonths(1);
            else if (granularity == Granularity.Week)
                return dt.AddDays(7);
            else if (granularity == Granularity.Day)
                return dt.AddDays(1);
            else if (granularity == Granularity.Hour)
                return dt.AddHours(1);
            else if (granularity == Granularity.Minute)
                return dt.AddMinutes(1);
            else if (granularity == Granularity.Second)
                return dt.AddSeconds(1);

            throw new ArgumentException("Given granularity couldn't be recognized.");
        }

        private bool CompareDate(DateTime first, DateTime second, Granularity granularity)
        {
            if (granularity == 0)
                if (first.Second != second.Second) return false;
            if ((int)granularity <= 1)
                if (first.Minute != second.Minute) return false;
            if ((int)granularity <= 2)
                if (first.Hour != second.Hour) return false;
            if ((int)granularity <= 3)
                if (first.Day != second.Day) return false;
            if ((int)granularity <= 4)
                if (first.Day > second.Day || first.AddDays(7) < second) return false;
            if ((int)granularity <= 5)
                if (first.Month != second.Month) return false;
            if ((int)granularity <= 6)
                if (first.Year != second.Year) return false;

            return true;
        }
    }
}
