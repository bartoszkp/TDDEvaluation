using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataFillStrategy;

namespace Domain.Services.Implementation.DataFillStrategy
{
    public class MonthFillDataStrategy : Domain.DataFillStrategy.DataFillStrategy
    {
        public MonthFillDataStrategy(MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }

        protected override void incrementData(ref DateTime date)
        {
            date = date.AddMonths(1);
        }
    }
}
