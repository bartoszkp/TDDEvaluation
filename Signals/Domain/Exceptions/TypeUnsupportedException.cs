using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class TypeUnsupportedException : Exception
    {
        public TypeUnsupportedException() : base("This DataType is unsupported, supported types: int, decimal, double, string, bool") { }
    }
}
