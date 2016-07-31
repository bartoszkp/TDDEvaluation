using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace DataAccess.AutoMappingOverrides.CustomTypes
{
    public class PathComponentsList : IUserType
    {
        public bool IsMutable {  get { return true; } }

        public Type ReturnedType {  get { return typeof(IEnumerable<string>); } }

        public SqlType[] SqlTypes { get { return new[] { new StringSqlType() }; } }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            return object.Equals(x, y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var r = rs[names[0]];

            return r == DBNull.Value
                ? Enumerable.Empty<string>()
                : Domain.Path.ParseComponents((string)r);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            object paramVal = DBNull.Value;
            if (value != null)
            {
                paramVal = Domain.Path.JoinComponents(((IEnumerable<string>)value));
            }

            var parameter = (IDataParameter)cmd.Parameters[index];
            parameter.Value = paramVal;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }
    }
}
