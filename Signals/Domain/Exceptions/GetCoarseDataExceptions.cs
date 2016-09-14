using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class GetCoarseDataGranularityExceptions : Exception
    {
        public GetCoarseDataGranularityExceptions()
             : base("Granularity mast be larger")
        {
        }
    }

    public class GetCoarseDataTypeExceptions : Exception
    {
        public GetCoarseDataTypeExceptions()
             : base("Type can`t be string or bool.")
        {
        }
    }
}
