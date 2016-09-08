using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ShadowSignalDataTypeOrGranularityDoesntMatch : Exception
    {
        public ShadowSignalDataTypeOrGranularityDoesntMatch()
            : base("ShadowSignal's DataType or Granularity doesnt match to Signal's DataType or Granularity")
        {
        }
    }
}
