using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotMatchingGranularitiesException : Exception
    {
        public NotMatchingGranularitiesException() :
            base("Granularity of signal and shadow signal are not the same")
        {

        }
    }
}
