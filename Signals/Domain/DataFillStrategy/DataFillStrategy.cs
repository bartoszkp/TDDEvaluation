using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.DataFillStrategy
{
    [NHibernateIgnore]
    public abstract class DataFillStrategy
    {
        protected MissingValuePolicyBase missingValuePolicy;

        protected abstract void incrementData(ref DateTime date);

        public void FillMissingData<T>(List<Domain.Datum<T>> datums, DateTime after, DateTime before)
        {
            var dict = new Dictionary<DateTime, Datum<T>>();
            var datum = new Datum<T>();
            if (this.missingValuePolicy is MissingValuePolicy.NoneQualityMissingValuePolicy<T>)
                datum = new Datum<T>()
                {
                    Quality = Quality.None,
                    Value = default(T),
                };
            else if (this.missingValuePolicy is MissingValuePolicy.SpecificValueMissingValuePolicy<T>)
            {
                var mvp = missingValuePolicy as MissingValuePolicy.SpecificValueMissingValuePolicy<T>;
                datum = new Datum<T>()
                {
                    Quality = mvp.Quality,
                    Value = mvp.Value,
                };

            }
            else if(this.missingValuePolicy is MissingValuePolicy.ZeroOrderMissingValuePolicy<T>)
            {
                if(datums.Find(d => d.Timestamp == after) == null)
                    dict.Add(after, new Datum<T>() { Quality = Quality.None, Value = default(T) });
                foreach (var item in datums)
                {
                    dict.Add(item.Timestamp, item);
                }
            }

            DateTime currentDate = new DateTime(after.Ticks);
            DateTime lastDate = currentDate;
            while (currentDate < before)
            {   
                if(datums.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null && this.missingValuePolicy is MissingValuePolicy.ZeroOrderMissingValuePolicy<T>)
                {
                    if (dict.ContainsKey(lastDate))
                        datum = dict[lastDate];
                    datums.Add(new Datum<T>()
                    {
                        Quality = datum.Quality,
                        Value = datum.Value,
                        Timestamp = currentDate
                    });
                }
                else if (datums.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                    datums.Add(new Datum<T>()
                    {
                        Quality = datum.Quality,
                        Value = datum.Value,
                        Timestamp = currentDate
                    });

                lastDate = currentDate;
                incrementData(ref currentDate);
            }
        }
    }

}
