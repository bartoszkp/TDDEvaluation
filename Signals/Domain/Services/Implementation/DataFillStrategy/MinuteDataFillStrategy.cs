﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Implementation.DataFillStrategy.Helpers.NoneQuality;

namespace Domain.Services.Implementation.DataFillStrategy
{
    public class MinuteDataFillStrategy : Domain.DataFillStrategy.DataFillStrategy
    {
        public MinuteDataFillStrategy(MissingValuePolicy.MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }


        public override void FillMissingData<T>(List<Datum<T>> datum, DateTime after, DateTime before)
        {
            NoneQualityMinuteDataFill.FillData(datum, after, before);
        }
    }
}
