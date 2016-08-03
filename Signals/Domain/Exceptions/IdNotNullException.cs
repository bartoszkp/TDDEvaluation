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

    public class PathIsEmptyOrNullException: Exception
    {
        public PathIsEmptyOrNullException(): base("Path can't be not null and empty")
        {

        }
    }
    
}
