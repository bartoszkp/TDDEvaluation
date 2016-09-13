using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService
{
    class GranularityNotGreaterThanSignalsGranularityException : Exception
    {
        public GranularityNotGreaterThanSignalsGranularityException() : base("Granularity parameter must be greater than signals's granularity")
        {

        }
    }
}
