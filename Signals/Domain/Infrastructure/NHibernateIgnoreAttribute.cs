using System;

namespace Domain.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NHibernateIgnoreAttribute : Attribute
    {
    }
}
