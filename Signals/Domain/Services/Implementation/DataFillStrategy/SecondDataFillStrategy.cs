using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Implementation.DataFillStrategy
{
    public class SecondDataFillStrategy : Domain.DataFillStrategy.DataFillStrategy
    {
        public SecondDataFillStrategy(MissingValuePolicy.MissingValuePolicyBase mvp)
        {
            this.missingValuePolicy = mvp;
        }

        protected override void incrementData(ref DateTime date)
        {
            date = date.AddSeconds(1);
        }
    }
}
