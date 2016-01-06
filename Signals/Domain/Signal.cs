using System;

namespace Signals.Domain
{
    public class Signal
    {
        public int Id { get; set; }

        public string DataType { get; set; }

        public Granularity Granularity { get; set; }

        public Path Path { get; set; }
    }
}
