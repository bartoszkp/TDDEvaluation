using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NoSuchSignalException : Exception
    {
        public NoSuchSignalException() : base("Signal with given id does not exist in the database")
        {

        }

    }
}
