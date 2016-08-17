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

            DateTime currentDate = new DateTime(after.Ticks);

            while (currentDate < before)
            {
                if (datums.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                    datums.Add(new Datum<T>()
                    {
                        Quality = datum.Quality,
                        Value = datum.Value,
                        Timestamp = currentDate
                    });

                incrementData(ref currentDate);
            }
        }
    }

}
