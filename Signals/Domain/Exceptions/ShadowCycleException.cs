using System;

namespace Domain.Exceptions
{
    public class ShadowCycleException : Exception
    {
        public ShadowCycleException()
            : base("There is a cycle")
        {
        }
    }
}
