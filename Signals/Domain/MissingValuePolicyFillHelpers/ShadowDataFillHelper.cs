using Domain;
using Domain.MissingValuePolicy;
using Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Infrastructure.NHibernateIgnore]
    class ShadowDataFillHelper : MissingValuePolicyFillHelper
    {
        public static List<Datum<T>> FillMissingData<T>(ShadowMissingValuePolicy<T> mvp, SignalsDomainService service,
            List<Datum<T>> data, DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);
            List<Datum<T>> shadowData = (List<Datum<T>>)service.GetData<T>(mvp.ShadowSignal, fromIncluded, toExcluded);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                    data.Add(shadowData.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0));

                currentDate = AddTime(currentDate, mvp.Signal.Granularity);
            }

            return data;
        }

    }
}
