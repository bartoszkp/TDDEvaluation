using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DataTypeException : Exception
    {
        public DataTypeException()
            : base("When setting ShadowMissingValuePolicy DataTypes must be the same")
        {
        }
    }
}
