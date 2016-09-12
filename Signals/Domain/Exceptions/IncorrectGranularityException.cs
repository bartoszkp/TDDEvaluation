using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncorrectGranularityException : Exception
    {
        public IncorrectGranularityException() :
            base("Granularity needs to be thinner than signal's granularity")
        { }
    }
}
