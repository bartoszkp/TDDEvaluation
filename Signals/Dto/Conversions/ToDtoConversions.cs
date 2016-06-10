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
                    var result = (T)TypeAdapter.Adapt(@this, @this.GetType(), derivedWithMatchingName);

                    return SetDataTypeIfNeeded(@this, result);
                }
            }

            return TypeAdapter.Adapt<T>(@this);
        }

        private static T SetDataTypeIfNeeded<T>(object source, T result)
        {
            if (!source.GetType().IsGenericType
                || source.GetType().GetGenericArguments().Length != 1)
            {
                return result;
            }

            var dataTypeProperty = result.GetType().GetProperty("DataType", typeof(Dto.DataType));

            if (dataTypeProperty == null)
            {
                return result;
            }

            var dataTypeValue = DataTypeUtils.FromNativeType(source.GetType().GetGenericArguments().Single()).ToDto<Dto.DataType>();

            dataTypeProperty.SetValue(result, dataTypeValue);

            return result;
        }
    }
}
