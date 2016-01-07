using System;

namespace Domain.Infrastructure
{
    public static class DataTypeUtils
    {
        public static Type GetNativeType(this DataType @this)
        {
            switch (@this)
            {
                case DataType.Boolean:
                    return typeof(bool);
                case DataType.Decimal:
                    return typeof(decimal);
                case DataType.Double:
                    return typeof(double);
                case DataType.Integer:
                    return typeof(int);
                case DataType.String:
                    return typeof(string);
                default:
                    throw new ArgumentException("Unknown data type", "@this");
            }
        }
    }
}
