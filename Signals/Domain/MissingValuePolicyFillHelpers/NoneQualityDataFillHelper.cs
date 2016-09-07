using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Infrastructure.NHibernateIgnore]
    public class NoneQualityDataFillHelper : MissingValuePolicyFillHelper
    {
        public static List<Datum<T>> FillMissingData<T>(Signal signal, 
            List<Datum<T>> data, DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = default(T),
                        Timestamp = currentDate,
                        Quality = Quality.None
                    };

                    data.Add(missingDatum);
                }

                currentDate = AddTime(currentDate, signal.Granularity);
            }

            return data;
        }
    }
}
