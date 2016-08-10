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
            int iterator = 0;
            DateTime date = res.First().Timestamp;

            while (iterator != res.Count())
            {
                var datum = (from x in res
                             where x.Timestamp == date
                             select x).FirstOrDefault();

                if (datum == null)
                {
                    var tempDatum = new Domain.Datum<T>()
                    {
                        Quality = Quality.None,
                        Signal = signal,
                        Timestamp = date,
                        Value = default(T)
                    };
                    res.Add(tempDatum);
                }

                if (signal.Granularity == Granularity.Second) date = date.AddSeconds(1);
                if (signal.Granularity == Granularity.Minute) date = date.AddMinutes(1);
                if (signal.Granularity == Granularity.Hour) date = date.AddHours(1);
                if (signal.Granularity == Granularity.Day) date = date.AddDays(1);
                if (signal.Granularity == Granularity.Week) date = date.AddDays(7);
                if (signal.Granularity == Granularity.Month) date = date.AddMonths(1);
                if (signal.Granularity == Granularity.Year) date = date.AddYears(1);

                iterator++;
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
      
    }
}
