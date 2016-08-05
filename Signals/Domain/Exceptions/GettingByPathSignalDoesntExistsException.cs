using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class GettingByPathSignalDoesntExistsException : Exception
    {
        public GettingByPathSignalDoesntExistsException()
            : base("You cannot get by path signal that doesn't exists")
        {
        }
    }
}
