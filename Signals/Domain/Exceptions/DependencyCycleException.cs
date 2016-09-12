using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DependencyCycleException : Exception
    {
        public DependencyCycleException() :
            base("Creation of dependency cycle is not allowed.")
        { }
    }
}
