namespace Dto.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy : MissingValuePolicy
    {
        public object Value { get; set; }

        public Quality Quality { get; set; }
    }
}
