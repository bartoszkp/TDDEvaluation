using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DatetimeIsInvalidException : Exception
    {
        public DatetimeIsInvalidException(DateTime dt, Granularity granularity)
            : base("Given timestamp was invalid because " + dt.ToString("yyyy:MM:dd HH:mm:ss") + " doesn't meet criteria of " + granularity.ToString()) { }
    }
}
