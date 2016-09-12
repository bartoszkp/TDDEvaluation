using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class CouldntGetASignalException : Exception
    {
        public CouldntGetASignalException() :
            base("Couldn't get a signal due to incorrect id or path")
        { }
    }
}
