using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class GetCoarseDataBadGranularityException : Exception
    {
        public GetCoarseDataBadGranularityException()
            : base("GetCoarseData() bad granularity exception")
        {
        }
    }
}
