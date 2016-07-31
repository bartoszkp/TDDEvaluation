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

        public static DataType FromNativeType(this Type @this)
        {
            if (@this.Equals(typeof(bool)))
                return DataType.Boolean;
            else if (@this.Equals(typeof(decimal)))
                return DataType.Decimal;
            else if (@this.Equals(typeof(double)))
                return DataType.Double;
            else if (@this.Equals(typeof(int)))
                return DataType.Integer;
            else if (@this.Equals(typeof(string)))
                return DataType.String;
            else
                throw new ArgumentException("Unknown data type", "@this");
        }
    }
}
