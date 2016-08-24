using System;

namespace Domain.Exceptions
{
    public class QuerryAboutDateWithIncorrectFormatException : Exception
    {
        public QuerryAboutDateWithIncorrectFormatException()
            : base("You cant querry about date time with incorrect format!")
        {
        }
    }
}
