using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Implementation.DataFillStrategy
{
    public class DayFillDataStrategy : Domain.DataFillStrategy.DataFillStrategy
    {
        public DayFillDataStrategy(MissingValuePolicy.MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }

        protected override void incrementData(ref DateTime date)
        {
            date = date.AddDays(1);
        }
    }
}
