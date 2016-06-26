using System;
using System.Linq;
using System.Reflection;

namespace Dto.Conversions
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class MapFromGenericDataTypeAttribute : Attribute
    {
        public static PropertyInfo GetSinglePropertyMappedFromGenericDataTypeOrNull(Type type)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SingleOrDefault(p => p.GetCustomAttributes(typeof(MapFromGenericDataTypeAttribute), false).Any());
        }
    }
}
