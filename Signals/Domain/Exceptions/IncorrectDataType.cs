using System;

namespace Domain.Exceptions
{
    public class IncorrectDataType : Exception
    {
        public IncorrectDataType(string dataType, string signalDataType)
            : base("Data of type '" + dataType + "' doesn't match signal's expected data type: '" + signalDataType + "'")
        {
        }
    }
}
