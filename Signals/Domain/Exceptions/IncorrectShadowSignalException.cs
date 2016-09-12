using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IncorrectShadowSignalException : Exception
    {
        public IncorrectShadowSignalException() 
            : base("ShadowSignal doesnt meet signal criteria.") { }
    }
}
