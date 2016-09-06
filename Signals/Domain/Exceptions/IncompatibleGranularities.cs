using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncompatibleGranularities : Exception
    {
        public IncompatibleGranularities()
            : base("Granularity of shadow signal and signal must be the same")
        {
        }
    }
}
