using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NoSuchSignalException : System.Exception
    {
        public NoSuchSignalException(string message) : base(message)
        {

        }
    }

    public class NoSuchGranularityException: Exception
    {
        public NoSuchGranularityException(): base("Wrong Granularity. Granularity must be less then granularity signal ")
        {

        }
    }

    public class NoSuchDataTypeException : Exception
    {
        public NoSuchDataTypeException() : base("Wrong DataType. DataType must be less then granularity signal ")
        {

        }
    }
}
