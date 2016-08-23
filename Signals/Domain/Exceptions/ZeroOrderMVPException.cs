using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ZeroOrderMVPException : Exception
    {
        public ZeroOrderMVPException()
            : base("ZeroOrderMissingValuePolicy do not operate on datum of null.")
        {}
    }
}
