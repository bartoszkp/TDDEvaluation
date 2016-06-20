using Dto.Conversions;

namespace Dto.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy : MissingValuePolicy
    {
        public object Value { get; set; }

        public Quality Quality { get; set; }

        [MapFromGenericDataType]
        public DataType DataType { get; set; }
    }
}
