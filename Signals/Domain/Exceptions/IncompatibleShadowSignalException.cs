using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncompatibleShadowSignalException : Exception
    {
        public IncompatibleShadowSignalException()
            : base("Shadow signal's granularity and datatype must match the parent signal.")
        {
        }
    }
}
