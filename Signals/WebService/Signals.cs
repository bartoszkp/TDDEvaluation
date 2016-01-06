using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Dto.Conversions;

namespace WebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class Signals : ISignals
    {
        private int currentId = 0;
        private Dictionary<string, Domain.Signal> signals = new Dictionary<string, Domain.Signal>();
        private Dictionary<int, Dictionary<DateTime, Domain.Datum<object>>> data = new Dictionary<int, Dictionary<DateTime, Domain.Datum<object>>>();
         
        public Dto.Signal Get(Dto.Path pathDto)
        {
            var path = pathDto.ToDomain<Domain.Path>();

            Domain.Signal s = null;
            if (signals.TryGetValue(path.ToString(), out s))
            {
                return s.ToDto<Dto.Signal>();
            }

            throw new FaultException(); // OR: return null;
        }

        public Dto.Signal Add(Dto.Signal signalDto)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            signal.Id = currentId++;

            signals[signal.Path.ToString()] = signal;

            return signal.ToDto<Dto.Signal>();
        }

        public IEnumerable<Dto.Datum> GetData(Dto.Signal signalDto, DateTime fromIncluded, DateTime toExcluded)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();

            var signalData = data[signal.Id.Value];

            foreach (var timestamp in new Domain.Infrastructure.TimeEnumerator(fromIncluded, toExcluded, signal.Granularity))
            {
                Domain.Datum<object> result = null;
                signalData.TryGetValue(timestamp, out result);

                yield return result.ToDto<Dto.Datum>();
            }
        }

        public void SetData(Dto.Signal signalDto, DateTime fromIncluded, IEnumerable<Dto.Datum> data)
        {
            var signal = signalDto.ToDomain<Domain.Signal>();
            var dataDict = data.ToDictionary(d => d.Timestamp, d => d.ToDomain<Domain.Datum<object>>());

            this.data[signal.Id.Value] = (new Domain.Infrastructure.TimeEnumerator(fromIncluded, dataDict.Count, signal.Granularity))
                .ToDictionary(ts => ts, ts => dataDict[ts]);
        }
    }
}
