using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    class TypeMismatchException : Exception
    {
        public TypeMismatchException()
            : base("The types must be equal")
        {
        }
    }
}
