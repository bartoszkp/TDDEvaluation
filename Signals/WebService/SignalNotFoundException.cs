using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService
{
    [Serializable]
    public class SignalNotFoundException : Exception
    {
        public SignalNotFoundException() : 
            base("Signal with the given ID not found.")
        {

        }
        public SignalNotFoundException(int id) : 
            base("Signal with the given ID not found, ID: " + id.ToString())
        {

        }
        public SignalNotFoundException(string msg)
        {

        }
        public SignalNotFoundException(string msg, Exception innerException) : base(msg, innerException)
        {

        }
    }
}
