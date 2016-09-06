using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncompatibleDataTypes : Exception
    {
        public IncompatibleDataTypes()
            : base("DataType of shadow signal and signal must be the same")
        {
        }
    }
}
