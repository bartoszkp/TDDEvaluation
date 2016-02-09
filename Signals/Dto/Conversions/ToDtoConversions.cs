using Mapster;

namespace Dto.Conversions
{
    public static class ToDtoConversions
    {
        public static T ToDto<T>(this object @this)
        {
            return TypeAdapter.Adapt<T>(@this);
        }
    }
}
