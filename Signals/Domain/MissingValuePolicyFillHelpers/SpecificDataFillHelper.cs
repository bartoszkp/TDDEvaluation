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
    public class SpecificDataFillHelper : MissingValuePolicyFillHelper
    {

        public static List<Datum<T>> FillMissingData<T>(SpecificValueMissingValuePolicy<T> mvp, 
            List<Datum<T>> data, DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = AddTime(currentDate, mvp.Signal.Granularity);
            }

            return data;
        }

    }
}
