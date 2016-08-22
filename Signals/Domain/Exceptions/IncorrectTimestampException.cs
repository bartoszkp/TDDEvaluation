using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncorrectTimestampException : Exception
    {
        public IncorrectTimestampException() 
            : base("Irregular timestamps for data are not allowed; set timestamps according to the given signal's granularity.")
        {

        }
    }
}
