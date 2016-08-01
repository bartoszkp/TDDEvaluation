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
            : base("Signal with this Id does not exist") { }
    }
}
