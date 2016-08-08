namespace Dto.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy : MissingValuePolicy
    {
        private object value = default(object);
        private Quality quality = Quality.None;

        public object Value { get { return this.value; } }
        public Quality Quality { get { return this.quality; } }
    }
}