using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidPolicyDataTypeException : Exception
    {
        public InvalidPolicyDataTypeException(Type type)
            : base($"`{type}` is not a valid data type for current MissingValuePolicy.")
        {

        }
    }
}
