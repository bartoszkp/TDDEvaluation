using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ShadowSignalDoesntMatchASignalException : Exception
    {
        public ShadowSignalDoesntMatchASignalException() :
            base("ShadowSignal's Granularity/DataType doesn't match signal's Granularity/DataType")
        { }
    }
}
