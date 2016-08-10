using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Implementation.DataFillStrategy.Helpers;

namespace Domain.Services.Implementation.DataFillStrategy
{
    public class HourFillDataStrategy : Domain.DataFillStrategy.DataFillStrategy
    {
        public HourFillDataStrategy(MissingValuePolicy.MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }

        public override void FillMissingData<T>(List<Datum<T>> datum, DateTime after, DateTime before)
        {
            NoneQualityHourDataFill.FillData(datum, after, before);
        }
    }
}
