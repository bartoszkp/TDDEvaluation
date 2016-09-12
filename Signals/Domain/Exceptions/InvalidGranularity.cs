using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    class InvalidGranularity : Exception
    {
        public InvalidGranularity()
            : base("Given granularity is not supported by this function.")
        {
        }
    }
}
