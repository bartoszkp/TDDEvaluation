using Domain.Infrastructure;
using Mapster;
using System;

namespace Dto.Conversions
{
    public static class ToDomainConversions
    {
        public static T ToDomain<T>(this object @this)
        {
            if (typeof(T).IsAbstract)
            {
                var derivedWithMatchingName = ReflectionUtils.GetSingleConcreteTypeWithGivenNameOrNull(typeof(T), @this.GetType().Name);

                if (derivedWithMatchingName != null)
                {
                    var dataTypeProperty = @this.GetType().GetProperty("DataType", typeof(Dto.DataType));

                    if (derivedWithMatchingName.IsGenericTypeDefinition
                        && derivedWithMatchingName.GetGenericArguments().Length == 1
                        && dataTypeProperty != null)
                    {
                        var dataType = TypeAdapter.Adapt<Domain.DataType>(dataTypeProperty.GetValue(@this));
                        var nativeDataType = dataType.GetNativeType();

                        derivedWithMatchingName = derivedWithMatchingName.MakeGenericType(nativeDataType);
                    }

                    return (T)TypeAdapter.Adapt(@this, @this.GetType(), derivedWithMatchingName);
                }
            }

            return TypeAdapter.Adapt<T>(@this);
        }

        public static object ToDomain(this object @this, Type targetType)
        {
            return TypeAdapter.Adapt(@this, @this.GetType(), targetType);
        }
    }
}
