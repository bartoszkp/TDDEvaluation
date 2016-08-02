using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SignalForDatumHasNotBeenSet : Exception
    {
        public SignalForDatumHasNotBeenSet() :
            base("Signal coundn't been set for Datum object")
        {
        }
    }
}
