using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataFillStrategy;
using Domain.Services.Implementation.DataFillStrategy.Helpers;

namespace Domain.Services.Implementation.DataFillStrategy
{
    public class MonthFillDataStrategy : Domain.DataFillStrategy.DataFillStrategy
    {


        public MonthFillDataStrategy(MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }


        public override void FillMissingData<T>(List<Datum<T>> datum, DateTime after, DateTime before)
        {
            if (this.missingValuePolicy is NoneQualityMissingValuePolicy<T>)
                NoneQualityMonthDataFill.FillData(datum, after, before);
        }
    }

}
