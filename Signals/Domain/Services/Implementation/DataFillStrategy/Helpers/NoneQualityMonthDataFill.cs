using Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Implementation.DataFillStrategy.Helpers
{
    [NHibernateIgnore]
    public static class NoneQualityMonthDataFill
    {
        public static void FillData<T>(List<Domain.Datum<T>> datum, DateTime after, DateTime before)
        {
            int currentMonth = after.Month + 1;

            if (after.Year == before.Year)
            {
                while (currentMonth < before.Month)
                {
                    var missingTimestamp = new DateTime(after.Year, currentMonth, after.Day);

                    if (datum.Find(d => DateTime.Compare(d.Timestamp, missingTimestamp) == 0) == null)
                        datum.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = new DateTime(after.Year, currentMonth, after.Day)
                        });

                    currentMonth++;

                }
            }

            else
            {
                int currentYear = after.Year;

                while (currentYear < before.Year)
                {
                    while (currentMonth <= 12)
                    {
                        var missingTimestamp = new DateTime(currentYear, currentMonth, after.Day);

                        if (datum.Find(d => DateTime.Compare(d.Timestamp, missingTimestamp) == 0) == null)
                            datum.Add(new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = missingTimestamp
                            });
                        currentMonth++;

                        if (currentMonth == before.Month - 1 && currentYear == before.Year)
                            return;

                    }
                    currentMonth = 1;
                    currentYear++;



                }
            }




        }
    }

}
