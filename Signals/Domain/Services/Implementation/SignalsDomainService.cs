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
            var result = signalsRepository.Add(newSignal);
            switch(result.DataType)
            {
                case (DataType.Boolean):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<bool>());
                    break;

                case (DataType.Decimal):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<decimal>());
                    break;

                case (DataType.Double):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<double>());
                    break;

                case (DataType.Integer):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<int>());
                    break;

                case (DataType.String):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<string>());
                    break;
            }
            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path signalPath)
        {
            if (signalPath == null)
                throw new ArgumentNullException("Attempted to get signal with null path");

            var result = this.signalsRepository.Get(signalPath);

            if (result == null)
                throw new NoSuchSignalException("Signal with given path does not exist in database");

            return result;
        }


        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException("Cannot get missing value policy for not exisitng signal");

            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException("Attempted to set missing value policy to a non exsisting signal");
            this.missingValuePolicyRepository.Set(signal, policy);

        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
        }

        public void SetData<T>(IEnumerable<Datum<T>> data, Signal signal)
        {
            if (data == null)
                throw new ArgumentNullException("Attempted to set null data for a signal");

            SetSignalForDatumCollection(data, signal);

            signalsDataRepository.SetData(data);
        }

        private void SetSignalForDatumCollection<T>(IEnumerable<Domain.Datum<T>> data, Signal signal)
        {
            if (!data.Any() || signal == null)
                return;

            foreach (var datum in data)
            {
                datum.Signal = signal;
            }
        }


    }
}
