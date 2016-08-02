using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class PathIsEmptyException: Exception
    {
        public PathIsEmptyException()
            : base("When adding new signal, Path of the signal can't be null.")
        {
        }
    }
}
