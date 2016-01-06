using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SignalsIntegrationTests
{
    public static class Utils
    {
        public delegate T Get<T>();

        public static void AssertReturnsNullOrThrows<T>(Get<T> fun)
        {
            try
            {
                Assert.IsNull(fun());
            }
            catch (FaultException)
            {}
        }
    }
}
