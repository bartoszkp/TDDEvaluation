﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotMatchingDataTypesException : Exception
    {
        public NotMatchingDataTypesException() :
            base("Data Types of signal and shadow signal are not the same.")
        {

        }
    }
}
