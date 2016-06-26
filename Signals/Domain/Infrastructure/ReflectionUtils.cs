using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Infrastructure
{
    public static class ReflectionUtils
    {
        public static Type GetSingleConcreteTypeWithMatchingNameOrNull(Type baseClass, string name)
        {
            return baseClass
                .Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(baseClass))
                .Where(t => NamesMatchIgnoringGenericPart(t.Name, name))
                .SingleOrDefault();
        }

        private static bool NamesMatchIgnoringGenericPart(string name1, string name2)
        {
            var i1 = name1.IndexOf('`');
            var i2 = name2.IndexOf('`');
            if (i1 >= 0)
            {
                name1 = name1.Substring(0, i1);
            }

            if (i2 >= 0)
            {
                name2 = name2.Substring(0, i2);
            }

            return name1 == name2;
        }

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
            var outermostExpression = expression.Body as MemberExpression;

            if (outermostExpression == null)
            {
                outermostExpression = TryAsUnaryExpression(expression);
            }

            if (outermostExpression == null)
            { 
                throw new ArgumentException("Invalid Expression. Expression should consist of a Property access only.");
            }

            return outermostExpression.Member;
        }

        private static MemberExpression TryAsUnaryExpression<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            var unaryExpression = expression.Body as UnaryExpression;

            MemberExpression result = null;

            if (unaryExpression != null)
            {
                result = unaryExpression.Operand as MemberExpression;
            }

            return result;
        }
    }
}
