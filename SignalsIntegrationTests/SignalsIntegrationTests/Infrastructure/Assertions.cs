using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public static class Then
    {
        public static void AssertEqual<T>(IEnumerable<Datum<T>> expected, IEnumerable<Datum<T>> actual)
        {
            CollectionAssert.AreEqual(
                expected.ToList(),
                actual.ToList(),
                Comparer<Datum<T>>.Create((x, y) => x.Value.Equals(y.Value) && x.Timestamp.Equals(y.Timestamp) ? 0 : 1));
        }
    }
}
