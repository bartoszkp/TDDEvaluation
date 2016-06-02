using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class Assertions
    {
        public static void AssertReturnsNullOrThrows<T>(Func<T> f) where T : class
        {
            try
            {
                Assert.IsNull(f());
            }
            catch (FaultException)
            {
            }
        }
    }
}
