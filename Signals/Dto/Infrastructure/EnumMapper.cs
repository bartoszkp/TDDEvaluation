using System;

namespace Dto.Infrastructure
{
    public static class EnumMapper
    {
        public static T MapEnum<S, T>(S source)
            where S : struct
            where T : struct
        {
            return (T)Enum.Parse(typeof(S), source.ToString());
        }
    }
}
