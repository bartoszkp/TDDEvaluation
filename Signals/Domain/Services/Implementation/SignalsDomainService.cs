using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Repositories;

namespace Domain.Services.Implementation
{
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalRepository signalRepository;

        private Dictionary<string, Signal> signals = new Dictionary<string, Signal>();
        private Dictionary<int, Dictionary<DateTime, Datum<object>>> data = new Dictionary<int, Dictionary<DateTime, Datum<object>>>();

        public SignalsDomainService(ISignalRepository signalRepository)
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
            return this.signalRepository.Add(signal);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var signalData = data[signal.Id];

            foreach (var timestamp in new Infrastructure.TimeEnumerator(fromIncluded, toExcluded, signal.Granularity))
            {
                Datum<object> result = null;
                signalData.TryGetValue(timestamp, out result);

                yield return result == null
                    ? null
                    : new Datum<T>() { Timestamp = result.Timestamp, Value = (T)result.Value };
            }
        }

        public void SetData<T>(Signal signal, DateTime fromIncluded, IEnumerable<Datum<T>> data)
        {
            var dataDict = data.ToDictionary(d => d.Timestamp, d => d);

            this.data[signal.Id] = (new Infrastructure.TimeEnumerator(fromIncluded, dataDict.Count, signal.Granularity))
                .ToDictionary(ts => ts, ts => new Datum<object>() { Timestamp = ts, Value = dataDict[ts].Value });
        }
    }
}
