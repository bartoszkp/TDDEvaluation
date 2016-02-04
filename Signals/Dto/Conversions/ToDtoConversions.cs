using FastMapper;
using System.Linq;

namespace Dto.Conversions
{
    public static class ToDtoConversions
    {
        public static T ToDto<T>(this object @this)
        {
            return TypeAdapter.Adapt<T>(@this);
        }

        public static Dto.Datum[] ToDto<T>(this Domain.Datum<T>[] @this)
        {
            return @this.Select(d => new Dto.Datum() { Timestamp = d.Timestamp, Value = d.Value }).ToArray();
        }
    }
}
