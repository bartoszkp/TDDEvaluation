using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncorrectDatumTimestampException : Exception
    {
        public IncorrectDatumTimestampException() :
            base("Timestamp of datum doesn't match signal granularity.")
        { }
    }
}
