using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Infrastructure
{
    public static class ReflectionUtils
    {
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            MethodCallExpression outermostExpression = expression.Body as MethodCallExpression;

            if (outermostExpression == null)
            {
                throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
            }

            return outermostExpression.Method;
        }

        public static MemberInfo GetMemberInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            MemberExpression outermostExpression = expression.Body as MemberExpression;

            if (outermostExpression == null)
            {
                throw new ArgumentException("Invalid Expression. Expression should consist of a Property access only.");
            }

            return outermostExpression.Member;
        }
    }
}
