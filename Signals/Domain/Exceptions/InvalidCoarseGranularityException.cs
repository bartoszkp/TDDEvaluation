using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidCoarseGranularityException : Exception
    {
        public InvalidCoarseGranularityException() : 
            base("Tried to get coarse data for granularity that is smaller than signal's.")
        {

        }
    }
}
