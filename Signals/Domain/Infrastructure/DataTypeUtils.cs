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

        public static DataType FromNativeType(Type nativeType)
        {
            if (nativeType.Equals(typeof(bool)))
            {
                return DataType.Boolean;
            }
            else if (nativeType.Equals(typeof(decimal)))
            {
                return DataType.Decimal;
            }
            else if (nativeType.Equals(typeof(double)))
            {
                return DataType.Double;
            }
            else if (nativeType.Equals(typeof(int)))
            {
                return DataType.Integer;
            }
            else if (nativeType.Equals(typeof(string)))
            {
                return DataType.String;
            }

            throw new ArgumentException("Unknown native type", "nativeType");
        }
    }
}
