using Domain;
using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFill.FillDataStrategy
{
    public class MonthFillDataStrategy : DataFillStrategy
    {


        public MonthFillDataStrategy(MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }
        

        public override void FillMissingData<T>(List<Datum<T>> datum, DateTime after, DateTime before)
        {
            if (missingValuePolicy is NoneQualityMissingValuePolicy<T>)
            {
                DateTime currentDate = new DateTime(after.Day, after.Month + 1, after.Day);

                int currentMonth = after.Month + 1;

                if (after.Year == before.Year)
                {
                    while (currentMonth < before.Month - 1)
                    {
                        datum.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate
                        });

                        currentMonth++;

                    }
                }

                else
                {
                    int currentYear = after.Year;

                    while (currentYear <= before.Year && currentMonth != before.Month - 1)
                    {

                        datum.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate
                        });

                        currentMonth++;

                        if (currentMonth == 13)
                        {
                            currentMonth = 1;
                            currentYear = before.Year;
                        }
                    }
                }

                

            }
        }
    }
}
