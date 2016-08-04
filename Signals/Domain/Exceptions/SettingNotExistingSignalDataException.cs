using System;

namespace Domain.Exceptions
{
    public class SettingNotExistingSignalDataException : Exception
    {
        public SettingNotExistingSignalDataException()
            : base("You cannot set data for signal that doesn't exists")
        {
        }
    }
}