using System;
using System.Collections.Generic;
using Domain.Repositories;
using Domain.Infrastructure;
using System.Linq;

namespace Domain.Services.Implementation
{
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalsRepository signalRepository;

        public SignalsDomainService(ISignalsRepository signalRepository)
        {
            this.signalRepository = signalRepository;
        }

        public Signal Get(Path path)
        {
            var result = this.signalRepository.Get(path);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }

            return result;
        }

        public Signal Add(Signal signal)
        {
            signal.MissingValuePolicyConfig = new MissingValuePolicyConfig();
            return this.signalRepository.Add(signal);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var readData = this.signalRepository.GetData<T>(signal, fromIncluded, toExcluded);

            return this.FillMissingData(signal, new TimeEnumerator(fromIncluded, toExcluded, signal.Granularity), readData);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            foreach (var d in data)
            {
                d.Signal = signal;
                signal.Granularity.ValidateTimestamp(d.Timestamp);
            }

            this.signalRepository.SetData<T>(data);
        }

        private IEnumerable<Datum<T>> FillMissingData<T>(Signal signal, TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : Datum<T>.CreateNone(signal, ts))
                .ToArray();
        }

        public MissingValuePolicyConfig GetMissingValuePolicyConfig(Signal signal)
        {
           return this.signalRepository.Get(signal.Path).MissingValuePolicyConfig;
        }

        public void SetMissingValuePolicyConfig(Signal signal, MissingValuePolicyConfig config)
        {
            signal = this.signalRepository.Get(signal.Path);
            signal.MissingValuePolicyConfig = config;
        }
    }
}
