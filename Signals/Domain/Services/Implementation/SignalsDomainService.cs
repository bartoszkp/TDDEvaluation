using System;
using System.Collections.Generic;
using Domain.Repositories;
using Domain.Infrastructure;
using System.Linq;
using Domain.Exceptions;

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

        public Signal Get(int signalId)
        {
            var result = this.signalRepository.Get(signalId);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }

            return result;
        }
  
        public Signal Add(Signal signal)
        {
            if (signal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            signal.MissingValuePolicyConfig = new MissingValuePolicyConfig();
            return this.signalRepository.Add(signal);
        }

        public PathEntry GetPathEntry(Path path)
        {
            var allSignals = this.signalRepository.GetAllWithPathPrefix(path);

            var directDescendants = allSignals.Where(s => s.Path.Length == path.Length + 1).ToArray();
            var subPaths = allSignals
                .Where(s => s.Path.Length > path.Length + 1)
                .Select(s => s.Path.GetPrefix(path.Length + 1))
                .Distinct()
                .ToArray();

            return new PathEntry(directDescendants, subPaths);
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
