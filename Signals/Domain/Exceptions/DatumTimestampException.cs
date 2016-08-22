using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DatumTimestampException : Exception
    {
        public DatumTimestampException()
            : base("Datum timestamp its not appropriate timestamp for signal granularity.")
        {
        }
    }
}
