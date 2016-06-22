using System;

namespace Domain.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true)]
    public class NHibernateIgnoreAttribute : Attribute
    {
    }
}
