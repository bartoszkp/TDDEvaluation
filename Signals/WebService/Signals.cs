using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Signals.Domain;

namespace Signals.WebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Signals : ISignals
    {
        private Dictionary<string, Signal> signals = new Dictionary<string, Signal>();
        private Dictionary<int, Dictionary<DateTime, Datum<object>>> data = new Dictionary<int, Dictionary<DateTime, Datum<object>>>();
         
        public Signal Get(Path path)
        {
            return signals[path.ToString()];
        }

        public Signal Add(Path path, string dataType, Granularity granularity)
        {
            var newSignal = new Signal()
            {
                Id = signals.Count,
                DataType = dataType,
                Granularity = granularity,
                Path = path
            };

            signals[path.ToString()] = newSignal;

            return newSignal;
        }

        public IEnumerable<Datum<object>> GetData(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var signalData = data[signal.Id];

            foreach (var timestamp in new TimeEnumerator(fromIncluded, toExcluded, signal.Granularity))
            {
                Datum<object> result = null;
                signalData.TryGetValue(timestamp, out result);

                yield return result;
            }
        }

        public void SetData(Signal signal, DateTime fromIncluded, IEnumerable<Datum<object>> data)
        {
            var dataDict = data.ToDictionary(d => d.Timestamp, d => d);

            this.data[signal.Id] = (new TimeEnumerator(fromIncluded, dataDict.Count, signal.Granularity))
                .ToDictionary(ts => ts, ts => dataDict[ts]);
        }
    }
}
