using Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Implementation.DataFillStrategy.Helpers
{
    static class NoneQualityMonthDataFill
    {
        public static void FillData<T>(List<Domain.Datum<T>> datum, DateTime after, DateTime before)
        {
            var currentDate = new DateTime(after.Ticks);

            while (currentDate < before)
            {
                if (datum.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    datum.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddMonths(1);

            }

        }
    }

}
