using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SignalNotExistException : Exception
    {
        public SignalNotExistException()
            : base("Could not get signal id or path is incorrect") { }
    }
}
