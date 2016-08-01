using System;

namespace Domain.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class NHibernateIgnoreAttribute : Attribute
    {
    }
}
