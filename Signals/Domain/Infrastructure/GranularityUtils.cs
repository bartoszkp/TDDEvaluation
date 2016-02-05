using System;

namespace Domain.Infrastructure
{
    public static class GranularityUtils
    {
        public static void ValidateTimestamp(this Granularity @this, DateTime timestamp)
        {
            switch (@this)
            {
                case Granularity.Second:
                    if (timestamp.Millisecond != 0)
                        throw new ArgumentException("Granularity.Second has unexpected Millisecond value", "timestamp");
                    break;
            }
        }
    }
}
