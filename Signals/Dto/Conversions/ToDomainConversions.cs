using System;
using System.Linq;
using Signals.Dto.Infrastructure;

namespace Signals.Dto.Conversions
{
    public static class ToDomainConversions
    {
        public static Signals.Domain.DataType ToDomain(this Signals.Dto.DataType @this)
        {
            return EnumMapper.MapEnum<Signals.Dto.DataType, Signals.Domain.DataType>(@this);
        }

        public static Signals.Domain.Granularity ToDomain(this Signals.Dto.Granularity @this)
        {
            return EnumMapper.MapEnum<Signals.Dto.Granularity, Signals.Domain.Granularity>(@this);
        }

        public static Signals.Domain.Datum<T> ToDomain<T>(this Signals.Dto.Datum @this)
        {
            return new Signals.Domain.Datum<T>()
            {
                Timestamp = @this.Timestamp,
                Value = (T)Convert.ChangeType(@this.Value, typeof(T))
            };
        }

        public static Signals.Domain.Path ToDomain(this Signals.Dto.Path @this)
        {
            return new Signals.Domain.Path()
            {
                Components = @this.Components.ToArray()
            };
        }

        public static Signals.Domain.Signal ToDomain(this Signals.Dto.Signal @this)
        {
            return new Signals.Domain.Signal()
            {
                Id = @this.Id,
                DataType = @this.DataType.ToDomain(),
                Granularity = @this.Granularity.ToDomain(),
                Path = @this.Path.ToDomain()
            };
        }
    }
}
