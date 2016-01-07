using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Implementation
{
    public class SignalsDomainService : ISignalsDomainService
    {
        private int currentId = 0;
        private Dictionary<string, Signal> signals = new Dictionary<string, Signal>();
        private Dictionary<int, Dictionary<DateTime, Datum<object>>> data = new Dictionary<int, Dictionary<DateTime, Datum<object>>>();

        public Signal Get(Path path)
        {
            Signal s = null;
            if (signals.TryGetValue(path.ToString(), out s))
            {
                return s;
            }

            throw new KeyNotFoundException(); // OR: return null;
        }

        public Signal Add(Signal signal)
        {
            signal.Id = currentId++;

            signals[signal.Path.ToString()] = signal;

            return signal;
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var signalData = data[signal.Id.Value];

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

            this.data[signal.Id.Value] = (new Infrastructure.TimeEnumerator(fromIncluded, dataDict.Count, signal.Granularity))
                .ToDictionary(ts => ts, ts => new Datum<object>() { Timestamp = ts, Value = dataDict[ts].Value });
        }
    }
}
