using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Calculations
{
    static class CompareDates
    { 
        public static int SecondDifference(DateTime olderTimestamp, DateTime newerTimestamp)
        {
            return Convert.ToInt32((olderTimestamp - newerTimestamp).TotalSeconds);
        }

        public static int MinuteDifference(DateTime olderTimestamp, DateTime newerTimestamp)
        {
            return Convert.ToInt32((olderTimestamp - newerTimestamp).TotalMinutes);
        }

        public static int HourDifference(DateTime olderDate, DateTime newerDate)
        {
            return Convert.ToInt32((olderDate - newerDate).TotalHours);
        }

        public static int DayDifference(DateTime olderDate, DateTime newerDate)
        {
            return Convert.ToInt32((olderDate - newerDate).TotalDays);
        }

        public static int WeekDifference(DateTime olderDate, DateTime newerDate)
        {
            return Convert.ToInt32((olderDate - newerDate).TotalDays) / 7;
        }
        
        public static int MonthDifference(DateTime olderValue, DateTime newerValue)
        {
            return (olderValue.Month - newerValue.Month) + 12 * YearDifference(olderValue, newerValue);
        }

        public static int YearDifference(DateTime olderValue, DateTime newerValue)
        {
            return olderValue.Year - newerValue.Year;
        }

    }
}
