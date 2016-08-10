using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class IdIsZeroException: Exception
    {
        public IdIsZeroException()
            : base("Id cannot be zero!")
        {
        }
    }
}
