using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidSignalTypeException : Exception
    {
        public InvalidSignalTypeException() : 
            base("Operation not valid for signals of boolean and string types")
        {

        }
    }
}
