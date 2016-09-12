using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotMatchingGranularities : Exception
    {
        public NotMatchingGranularities() :
            base("Granularity of signal and shadow signal are not the same")
        {

        }
    }
}
