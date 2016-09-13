using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidDataTypeException : Exception
    {
        public InvalidDataTypeException() :
            base("Only Boolean, Int32, Double, Decimal and String are supported")
        { }
    }
}
