namespace Domain
{
    public class Signal
    {
        public virtual int? Id { get; set; }

        public virtual DataType DataType { get; set; }

        public virtual Granularity Granularity { get; set; }

        public virtual Path Path { get; set; }

        public virtual MissingValuePolicy.MissingValuePolicy MissingValuePolicy { get; set; }
    }
}
