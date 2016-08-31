using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SignalDoesNotExist : Exception
    {
        public SignalDoesNotExist()
            : base("Signal with given signal id does not exist.")
        {
        }
    }
}
