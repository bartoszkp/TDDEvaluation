using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BadDataTypeException : Exception
    {
        public BadDataTypeException()
            : base("Bad Signal's DataType Exception")
        {
        }
    }
}
