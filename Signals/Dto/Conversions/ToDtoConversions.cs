using Domain.Infrastructure;
using Mapster;

namespace Dto.Conversions
{
    public static class ToDtoConversions
    {
        public static T ToDto<T>(this object @this)
        {
            if (typeof(T).IsAbstract)
            {
                var derivedWithMatchingName = ReflectionUtils.GetSingleConcreteTypeWithMatchingNameOrNull(typeof(T), @this.GetType().Name);

                if (derivedWithMatchingName != null)
                {
                    return (T)TypeAdapter.Adapt(@this, @this.GetType(), derivedWithMatchingName);
                }
                else
                {
                    var parent = @this.GetType().BaseType;
                    var derivedParentWithMatchingName = ReflectionUtils.GetSingleConcreteTypeWithMatchingNameOrNull(typeof(T), parent.Name);

                    if (derivedParentWithMatchingName != null)
                    {
                        var result = (T)TypeAdapter.Adapt(@this, @this.GetType(), derivedParentWithMatchingName);
                        if (@this is Domain.MissingValuePolicy.MissingValuePolicyBase)
                            (result as MissingValuePolicy.MissingValuePolicy).DataType = 
                                (@this as Domain.MissingValuePolicy.MissingValuePolicyBase).NativeDataType.FromNativeType().ToDto<DataType>();
                        return result;
                    }
                }
            }

            return TypeAdapter.Adapt<T>(@this);
        }
    }
}
