using Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Infrastructure.NHibernateIgnore]
    public class ZeroOrderDataFillHelper : MissingValuePolicyFillHelper
    {

        public static List<Datum<T>> FillMissingData<T>(SignalsDomainService service, Signal signal, 
            List<Datum<T>> data, DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = AddTime(currentDate, signal.Granularity);
            }

            return data;
        }
    }
}
