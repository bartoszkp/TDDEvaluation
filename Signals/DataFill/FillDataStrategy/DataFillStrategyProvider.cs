using Domain;
using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFill.FillDataStrategy
{
    public static class DataFillStrategyProvider
    {
        public static DataFillStrategy GetStrategy(Granularity granularity, MissingValuePolicyBase mvp)
        {
            switch (granularity)
            {
                /*case Granularity.Second:

                    break;
                case Granularity.Minute:

                    break;
                case Granularity.Hour:
                    break;

                case Granularity.Day:
                    break;

                case Granularity.Week:
                    break;*/

                case Granularity.Month:
                    return new MonthFillDataStrategy(mvp);

                    

               /* case Granularity.Year:
                    break;*/

                default:
                    return null;
            }
        }


    }
}
