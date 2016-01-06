using System;
using System.Linq;
using Signals.Dto.Infrastructure;

namespace Signals.Dto.Conversions
{
    public static class ToDtoConversions
    {
        public static Signals.Dto.DataType ToDto(this Signals.Domain.DataType @this)
        {
            return EnumMapper.MapEnum<Signals.Domain.DataType, Signals.Dto.DataType>(@this);
        }

        public static Signals.Dto.Granularity ToDto(this Signals.Domain.Granularity @this)
        {
            return EnumMapper.MapEnum<Signals.Domain.Granularity, Signals.Dto.Granularity>(@this);
        }

        public static Signals.Dto.Datum ToDto<T>(this Signals.Domain.Datum<T> @this)
        {
            return new Signals.Dto.Datum()
            {
                Timestamp = @this.Timestamp,
                Value = @this.Value
            };
        }

        public static Signals.Dto.Path ToDto(this Signals.Domain.Path @this)
        {
            return new Signals.Dto.Path()
            {
                Components = @this.Components.ToArray()
            };
        }

        public static Signals.Dto.Signal ToDto(this Signals.Domain.Signal @this)
        {
            return new Signals.Dto.Signal()
            {
                Id = @this.Id,
                DataType = @this.DataType.ToDto(),
                Granularity = @this.Granularity.ToDto(),
                Path = @this.Path.ToDto()
            };
        }
    }
}
