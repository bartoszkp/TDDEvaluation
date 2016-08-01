using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NoSuchSignalException : System.Exception
    {
        public NoSuchSignalException() : base("Signal with given path does not exist in database")
        {

        }
    }
}
