using System;

namespace Domain.Exceptions
{
    public class SettingPolicyNotExistingSignalException : Exception
    {
        public SettingPolicyNotExistingSignalException()
            : base("You cannot set missing value policy for signal that doesn't exists")
        {
        }
    }
}