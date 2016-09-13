using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ShadowDependencyCycleException : Exception
    {
        public ShadowDependencyCycleException() : 
            base("Tried to set a signal as a shadow that would create a dependency cycle.")
        {

        }
    }
}
