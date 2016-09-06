using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NoSuchSignalException : Exception
    {
        public NoSuchSignalException()
            :base ("Signal with this id does not exist")
        {

        }
    }

    public class NonExistMissingValuePolicy : Exception
    {
        public NonExistMissingValuePolicy() : base("MissingValuePolicy non exist")
        {
        }
    }

    public class TimestampHaveWrongFormatException:Exception
    {
        public TimestampHaveWrongFormatException():base("Wrong format for Timestamp")
        {

        }
    }
}
