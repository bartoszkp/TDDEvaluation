using Domain.Infrastructure;
using Mapster;
using System;
using System.Linq;

namespace Dto.Conversions
{
    public static class ToDtoConversions
    {
        public static T ToDto<T>(this object @this)
        {
            if (typeof(T).IsAbstract)
            {
                var derivedWithMatchingName = ReflectionUtils
                    .GetSingleConcreteTypeWithGivenNameOrNull(typeof(T), @this.GetType().GetNameWithoutArity());

                if (derivedWithMatchingName != null)
                {
                    return (T)TypeAdapter.Adapt(@this, @this.GetType(), derivedWithMatchingName);
                }
            }

            return TypeAdapter.Adapt<T>(@this);
        }
    }
}
