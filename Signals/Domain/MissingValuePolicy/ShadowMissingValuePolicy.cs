using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }
                
        public virtual IEnumerable<Datum<T>> FillData(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc,
            IEnumerable<Datum<T>> data, IEnumerable<Datum<T>> shadowData)
        {
            var dataTab = data.OrderBy(datum => datum.Timestamp).ToArray();
            var shadowTab = shadowData.ToArray();
            List<Datum<T>> filledData = new List<Datum<T>>();

            int dataIndex = 0;
            int shadowIndex = 0;
            for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc; ++shadowIndex, currentDate = AddTime(signal.Granularity, currentDate))
            {
                if (dataIndex<dataTab.Length && dataTab[dataIndex].Timestamp == currentDate)
                    filledData.Add(dataTab[dataIndex++]);
                else
                    filledData.Add(shadowTab[shadowIndex]);
            }
            return filledData;
        }
    }
}
