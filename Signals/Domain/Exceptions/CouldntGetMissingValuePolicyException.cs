using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class CouldntGetMissingValuePolicyException : Exception
    {
        public CouldntGetMissingValuePolicyException() :
            base("Couldn't get Missing Value Policy, due to non existing signal")
        { }
    }
}
