using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SignalWithThisIdNonExistException : Exception
    {
        public SignalWithThisIdNonExistException()
            :base ("Signal with this id non exist")
        {

        }
    }
}
