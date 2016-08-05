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

            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            if (result == null)
            {
                throw new SignalWithThisPathNonExistException();
            }
            return result;
        }

        public MissingValuePolicyBase GetMissingValuePolicyBase(int signalId)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
            {
                throw new SignalWithThisIdNonExistException();
            }
            var mvp = missingValuePolicyRepository.Get(signal);
            if (mvp == null)
                return null;
            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicyBase(int signalId, MissingValuePolicyBase policy)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
            {
                throw new SignalWithThisIdNonExistException();
            }
            missingValuePolicyRepository.Set(signal, policy);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new SignalWithThisIdNonExistException();
            return signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> dataDomain)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new SignalWithThisIdNonExistException();
            //dataDomain.Count();
            //int rozmiar = dataDomain.Count();
            //var datum = new Datum<T> [rozmiar];
            //int i = 0;
            var datums = new List<Datum<T>>();

            foreach (Datum<T> d in dataDomain)
            {
                datums.Add( new Datum<T>
                {
                    Quality = d.Quality,
                    Timestamp = d.Timestamp,
                    Value = d.Value,
                    Signal = signal
                });
            }
            signalsDataRepository.SetData<T>(datums);
        }
    }
}
