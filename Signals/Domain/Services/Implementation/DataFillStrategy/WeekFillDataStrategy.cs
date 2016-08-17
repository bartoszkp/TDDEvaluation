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

        protected override void incrementData(ref DateTime date)
        {
            date = date.AddDays(7);
        }
    }
}
