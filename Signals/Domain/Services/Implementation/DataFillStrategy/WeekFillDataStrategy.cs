using Domain.Services.Implementation.DataFillStrategy.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Implementation.DataFillStrategy
{
    class WeekFillDataStrategy : Domain.DataFillStrategy.DataFillStrategy
    {
        public WeekFillDataStrategy(MissingValuePolicy.MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }


        public override void FillMissingData<T>(List<Datum<T>> datum, DateTime after, DateTime before)
        {
            if (this.missingValuePolicy is MissingValuePolicy.NoneQualityMissingValuePolicy<T>)
                NoneQualityWeekDataFill.FillData(datum, after, before);
        }
    }
}
