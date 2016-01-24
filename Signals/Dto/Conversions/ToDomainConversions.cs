using System;
using FastMapper;

namespace Dto.Conversions
{
    public static class ToDomainConversions
    {
        public static T ToDomain<T>(this object @this)
        {
            return TypeAdapter.Adapt<T>(@this);
        }

        public static object ToDomain(this object @this, Type targetType)
        {
            return TypeAdapter.Adapt(@this, @this.GetType(), targetType);
        }
    }
}
