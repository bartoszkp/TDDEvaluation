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


        public Signal Add(Signal newSignal, MissingValuePolicyBase policy)
        {
            var res = this.signalsRepository.Add(newSignal);
            try
            {
                missingValuePolicyRepository.Set(newSignal, policy);
            }
            catch { }
            return res;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var res = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(x => x.Timestamp).ToList();
            var mvp = Get(signal);

            if (mvp == null) mvp = new NoneQualityMissingValuePolicy<T>();

            while(fromIncludedUtc < toExcludedUtc)
            {
                var datum = (from x in res
                             where x.Timestamp == fromIncludedUtc
                             select x).FirstOrDefault();

                if (datum == null)
                {
                    var tempDatum = (mvp as MissingValuePolicy<T>).GetDatum();
                    tempDatum.Signal = signal;
                    tempDatum.Timestamp = fromIncludedUtc;

                    res.Add(tempDatum);
                }

                fromIncludedUtc = AddTime(signal.Granularity, fromIncludedUtc);
            }
             
            return res.OrderBy(x => x.Timestamp);
        }

        public void Set(Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            missingValuePolicyRepository.Set(signal, missingValuePolicy);
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

        private DateTime AddTime(Granularity granuality,DateTime date)
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
      
    }
}
