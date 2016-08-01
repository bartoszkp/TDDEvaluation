using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SignalIsNullException : Exception
    {
        public SignalIsNullException()
            : base("Signal is null due to, using incorrect/non-existing path or id")
        {
        } 
    }
}
