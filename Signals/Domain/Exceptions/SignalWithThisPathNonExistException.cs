using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SignalWithThisPathNonExistException : Exception
    {
        public SignalWithThisPathNonExistException()
            :base ("Signal with this path non exist")
        {

        }
    }
}
