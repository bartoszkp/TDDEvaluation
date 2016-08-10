using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Implementation.DataFillStrategy;

namespace Domain.DataFillStrategy
{
    public static class DataFillStrategyProvider
    {
        public static DataFillStrategy GetStrategy(Granularity granularity,MissingValuePolicy.MissingValuePolicyBase mvp)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    break;
                    
                case Granularity.Minute:
                    break;
                case Granularity.Hour:
                    break;

                case Granularity.Day:
                    break;
                    
                case Granularity.Week:
                    return new WeekFillDataStrategy(mvp);

                case Granularity.Month:
                    return new MonthFillDataStrategy(mvp);

                case Granularity.Year:
                    return new YearFillDataStrategy(mvp);

                default:
                    break;
            }

            return null;
        }


    }
}
