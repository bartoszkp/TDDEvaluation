﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Implementation.DataFillStrategy.Helpers
{
    static class NoneQualityWeekDataFill
    {
        public static void FillData<T>(List<Domain.Datum<T>> datum, DateTime after, DateTime before)
        {
            int currentYear = after.Year;
            int currentMonth = after.Month;
            int currentDay = after.Day;
            int afterSevenDays = currentDay + 7;

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
                    
                
                currentDate = currentDate.AddDays(7);
                Debug.WriteLine(currentDate);
            }


      

        }

    }

}
