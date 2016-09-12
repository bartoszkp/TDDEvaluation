using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ShadowMissingValuePolicyDataTypeException : Exception
    {
        public ShadowMissingValuePolicyDataTypeException()
            : base("When setting ShadowMissingValuePolicy DataTypes must be the same")
        {
        }
    }
    public class ShadowMissingValuePolicyGranularityException : Exception
    {
        public ShadowMissingValuePolicyGranularityException()
            : base("When setting ShadowMissingValuePolicy Granularity must be the same")
        {
        }
    }
    public class ShadowMissingValuePolicyCycleException : Exception
    {
        public ShadowMissingValuePolicyCycleException()
            : base("Assigning this policy will cause a dependency cycle of signals")
        {
        }
    }
}
