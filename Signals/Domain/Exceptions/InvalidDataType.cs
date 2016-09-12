using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidDataType : Exception
    {
        public InvalidDataType()
            : base("Given datatype is not supported by this function.")
        {
        }
    }
}