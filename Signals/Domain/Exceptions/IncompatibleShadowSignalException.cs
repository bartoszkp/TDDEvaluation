using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncompatibleShadowSignalException : Exception
    {
        public IncompatibleShadowSignalException() : 
            base("Tried to set a signal as a shadow that isn't compatible with the parent signal in terms of data type and/or granularity.")
        {

        }

    }
}
