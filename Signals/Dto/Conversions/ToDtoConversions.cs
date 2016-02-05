using FastMapper;
using System;
using System.Linq;

namespace Dto.Conversions
{
    public static class ToDtoConversions
    {
        public static T ToDto<T>(this object @this)
        {
            return TypeAdapter.Adapt<T>(@this);
        }

        public static Dto.Datum ToDto<T>(this Domain.Datum<T> @this)
        {
            return new Dto.Datum()
            {
                Timestamp = @this.Timestamp,
                Value = @this.Value,
                Quality = (Dto.Quality)@this.Quality
            };
        }

        public static Dto.Datum[] ToDto<T>(this Domain.Datum<T>[] @this)
        {
            return @this.Select(d => d.ToDto()).ToArray();
        }
    }
}
