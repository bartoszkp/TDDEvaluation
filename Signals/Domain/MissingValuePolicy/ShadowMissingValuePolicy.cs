using Domain.Services.Implementation;
using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc,
            Datum<T> olderDatum = null, Datum<T> neverDatum = null, SignalsDomainService service = null)
        {
            var currentDate = new DateTime(fromIncludedUtc.Ticks);
            var result = new List<Datum<T>>(data);
            List<Datum<T>> shadowData = (List<Datum<T>>) service.GetData<T>(this.ShadowSignal, fromIncludedUtc, toExcludedUtc);

            if(fromIncludedUtc == toExcludedUtc)
            {
                if (result.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    result.Add(shadowData.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0));
                }
            }

            
            while (currentDate < toExcludedUtc)
            {
                if (result.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                    result.Add(shadowData.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0));

                currentDate = AddTime(currentDate, this.Signal.Granularity);
            }

            return result;
        }
    }
}
