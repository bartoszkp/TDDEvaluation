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
            CheckDatasDateTimeCorrection(domainData);

            signalsDataRepository.SetData<T>(domainData);
        }
   

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            CheckTimestampsCorrection(signal, fromIncludedUtc);

            var getData = signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc)
                ?.OrderBy(s => s.Timestamp).ToArray();

            var getMissingValuePolicy = GetMissingValuePolicy(signal);

            if (getMissingValuePolicy == null)
                return getData;
            
     
            var result = new List<Datum<T>>();
            var dt = fromIncludedUtc;

            while(dt < toExcludedUtc )
            {
                var next = SignalUtils.GetNextDate(dt, signal.Granularity);

                var datum = getData.FirstOrDefault(d => dt <= d.Timestamp && next > d.Timestamp);
                if (datum == null)
                    datum = GenerateDatumFromPolicy(getMissingValuePolicy as MissingValuePolicy<T>, signal, dt);
                result.Add(datum);

                dt = next;
            }
            return result;
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(pathDomain);

            var filteredSignals = signals.Where(s => s.Path.Length == pathDomain.Length + 1);
            var subFolders = signals
                .Where(s => s.Path.Length > pathDomain.Length + 1)
                .Select(s => s.Path.GetPrefix(pathDomain.Length + 1))
                .Distinct();

            var result = new PathEntry(filteredSignals, subFolders);

            return result;
        }

        private void CheckDatasDateTimeCorrection<T>(IEnumerable<Datum<T>> data)
        {
            foreach (var d in data)
            {
                CheckTimestampsCorrection(d.Signal, d.Timestamp);
            }
        }

        private void CheckTimestampsCorrection(Signal signal, DateTime dateTime)
        {
            if ((signal.Granularity == Granularity.Second && dateTime.Millisecond != 0)
                || (signal.Granularity == Granularity.Minute && (dateTime.Second != 0 || dateTime.Millisecond != 0) )
                || (signal.Granularity == Granularity.Hour && (dateTime.Minute != 0 || dateTime.Second != 0 || dateTime.Millisecond != 0) )
                || (signal.Granularity == Granularity.Day && dateTime.TimeOfDay.Ticks != 0)
                || (signal.Granularity == Granularity.Week && (dateTime.TimeOfDay.Ticks != 0 || dateTime.DayOfWeek != DayOfWeek.Monday) )
                || (signal.Granularity == Granularity.Month
                    && (dateTime.TimeOfDay.Ticks != 0 || DateTime.Compare(dateTime, new DateTime(dateTime.Year, dateTime.Month, 1)) != 0))
                || (signal.Granularity == Granularity.Year 
                    && (dateTime.TimeOfDay.Ticks != 0 || DateTime.Compare(dateTime, new DateTime(dateTime.Year, 1, 1)) != 0)) )
                throw new ArgumentException("DateTime is incorrect");
        }

        private Datum<T> GenerateDatumFromPolicy<T>(MissingValuePolicy<T> mvp, Signal signal, DateTime timestamp)
        {
            var result = new Datum<T>()
            {
                Signal = signal,
                Timestamp = timestamp
            };

            return mvp.FillMissingValue(result);
        }
    }
}
