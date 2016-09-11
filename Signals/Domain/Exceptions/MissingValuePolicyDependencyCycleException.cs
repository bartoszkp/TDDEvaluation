using System;

namespace Domain.Exceptions
{
    public class MissingValuePolicyDependencyCycleException : Exception
    {
        public MissingValuePolicyDependencyCycleException()
            : base("Cannot create cycle of dependency with missing value policies")
        {
        }
    }
}
