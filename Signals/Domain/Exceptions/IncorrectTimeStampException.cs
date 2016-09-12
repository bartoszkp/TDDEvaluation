using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncorrectTimeStampException : Exception
    {
        public IncorrectTimeStampException() :
            base("TimeStamp has incorrect data due in reference to Granularity")
        { }
    }
}
