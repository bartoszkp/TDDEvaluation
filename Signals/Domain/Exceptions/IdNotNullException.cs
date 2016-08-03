using System;

namespace Domain.Exceptions
{
    public class IdNotNullException : Exception
    {
        public IdNotNullException()
            : base("When adding new signal, Id must be null")
        {
        }

        
    }
    public class PathNotExistException : Exception
    {
        public PathNotExistException() : base("Signal with this Path is not Exist")
        {

        }
    }

    public class SignalIsNotException: Exception
    {
        public SignalIsNotException(): base("Sighnal with this Id is not Exist")
        {

        }
    }
    
}
