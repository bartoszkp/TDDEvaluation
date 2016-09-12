using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidDataTypeForGetCoarseDataException : Exception
    {
        public InvalidDataTypeForGetCoarseDataException() :
            base("Couldn't get coarse data for string/boolean")
        { }
    }
}
