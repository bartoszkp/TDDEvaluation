using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Implementation.DataFillStrategy.Helpers
{
    public static class NoneQualityYearDataFill
    {
        public static void FillData<T>(List<Domain.Datum<T>> datum, DateTime after, DateTime before)
        {
            int currentYear = after.Year + 1;

            while (currentYear < before.Year)
            {
                var missingTimestamp = new DateTime(currentYear, after.Month, after.Day);

             //   if (datum.Find(d => d.Timestamp.Date == missingTimestamp.Date) != null) 
                if (datum.Find(d => DateTime.Compare(d.Timestamp,missingTimestamp) == 0) == null)
                    datum.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = missingTimestamp
                    });

                    currentYear++;
            }

        }


       

    }
}
