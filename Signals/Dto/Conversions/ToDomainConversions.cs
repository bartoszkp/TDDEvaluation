using FastMapper;

namespace Dto.Conversions
{
    public static class ToDomainConversions
    {
        public static T ToDomain<T>(this object @this)
        {
            return TypeAdapter.Adapt<T>(@this);
        }
    }
}
