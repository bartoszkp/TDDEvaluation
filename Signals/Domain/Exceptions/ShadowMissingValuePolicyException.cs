using System;

namespace Domain.Exceptions
{
    public class ShadowMissingValuePolicyException : Exception
    {
        public ShadowMissingValuePolicyException()
             : base("Datatype and granularity of ShadowMissingValuePolicy and Signal doesnt match")
        {
        }
    }

    public class ShadowMissingCyclePolicyException : Exception
    {
        public ShadowMissingCyclePolicyException()
             : base("Signal already used in some shadows")
        {
        }
    }
}
