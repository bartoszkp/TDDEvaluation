using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SignalNotFoundException: Exception
    {
        public SignalNotFoundException()
            : base("Signal of the given Id was not found")
        {
        }
    }
}
