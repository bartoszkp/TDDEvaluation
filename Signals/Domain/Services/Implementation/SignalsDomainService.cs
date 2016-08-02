using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
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

        public Signal GetById(int signalId)
        {
            var result = this.signalsRepository.Get(signalId);
            if (result == null) throw new InvalidSignalId();
            else return result;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            return this.signalsRepository.Add(newSignal);
        }

        public Signal Get(Path pathDto)
        {
            var result = this.signalsRepository.Get(pathDto);
            if (result == null) throw new InvalidPathArgument();
            else return result;
        }

        public void SetData(IEnumerable<Datum<double>> newDomainDatum)
        {
            this.signalsDataRepository.SetData(newDomainDatum);
        }

        public IEnumerable<Datum<double>> GetData(Signal getSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var result = this.signalsDataRepository.GetData<double>(getSignal, fromIncludedUtc, toExcludedUtc);
            var newDatum = new List<Datum<double>>();
            foreach (var f in result)
            {
                newDatum.Add(new Datum<double>() { Id = f.Id, Quality = f.Quality, Signal = f.Signal, Timestamp = f.Timestamp, Value = f.Value });
            }
            return newDatum;
        }
    }
}
