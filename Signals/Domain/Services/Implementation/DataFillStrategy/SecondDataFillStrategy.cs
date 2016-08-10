using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Implementation.DataFillStrategy.Helpers.NoneQuality;

namespace Domain.Services.Implementation.DataFillStrategy
{
    public class SecondDataFillStrategy : Domain.DataFillStrategy.DataFillStrategy
    {
        public SecondDataFillStrategy(MissingValuePolicy.MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }

        public override void FillMissingData<T>(List<Datum<T>> datum, DateTime after, DateTime before)
        {
            NoneQualitySecondDataFill.FillData(datum, after, before);
        }
    }
}
