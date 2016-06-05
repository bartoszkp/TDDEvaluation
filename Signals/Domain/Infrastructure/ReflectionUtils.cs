﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Infrastructure
{
    public static class ReflectionUtils
    {
        public static Type GetSingleConcreteTypeWithGivenNameOrNull(Type baseClass, string name)
        {
            return baseClass
                .Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(baseClass))
                .Where(t => t.GetNameWithoutArity() == name)
                .SingleOrDefault();
        }

        public static string GetNameWithoutArity(this Type @this)
        {
            int arityIndex = @this.Name.IndexOf('`');
            return arityIndex == -1 ? @this.Name : @this.Name.Substring(0, arityIndex);
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
