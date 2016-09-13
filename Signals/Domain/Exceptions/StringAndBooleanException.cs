using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class StringAndBooleanException : Exception
    {
        public StringAndBooleanException() :
            base("Current operation is not supported for string/boolean data type")
        { }
    }
}
