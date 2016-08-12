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
            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository?.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result; 
            }
        }

        public void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository?.Get(signal);
            if (mvp == null)
            {
                return null;
            }
            else
            {
                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
            }
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum)
        {
            if(datum == null)
            {
                this.signalsDataRepository.SetData<T>(datum);
                return;
            }

            foreach(var d in datum)
            {
                d.Signal = signal;
            }

            this.signalsDataRepository.SetData<T>(datum);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            
                return this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
            /*if (GetMissingValuePolicy(signal).GetType().Name.Contains("NoneQualityMissingValuePolicy"))
            {
                /*var gettingList = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
                var returnList = new List<Datum<T>>();
                var granulitary = signal.Granularity;
                if (granulitary == Granularity.Day) ;
                {
                    TimeSpan difference = toExcludedUtc - fromIncludedUtc;
                    DateTime checkedDateTime = fromIncludedUtc;
                    int countElementOfList = difference.Days;
                    for (int i = 0; i < countElementOfList; i++)
                    {

                        Datum<T> xx = gettingList.FirstOrDefault(x => x.Timestamp == checkedDateTime);
                        if (xx == null)
                        {
                            var addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                            returnList.Add(addingItem);

                        }
                        else
                        {
                            returnList.Add(xx);
                        }
                        checkedDateTime = checkedDateTime.AddDays(1);
                    }
                }
                return returnList;
                throw new NotImplementedException();
            }
            else*/


        }
    }
}
