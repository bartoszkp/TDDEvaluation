using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        public static void AssertThrows(Action f)
        {
            try
            {
                f();
                throw new AssertFailedException();
            }
            catch (FaultException)
            {
            }
        }

        public static void AssertEqual<T>(IEnumerable<Domain.Datum<T>> expected, IEnumerable<Domain.Datum<T>> actual)
        {
            CollectionAssert.AreEqual(
                expected.ToList(),
                actual.ToList(),
                Comparer<Domain.Datum<T>>.Create((x, y) => x.Value.Equals(y.Value) 
                                                            && x.Quality == y.Quality
                                                            && x.Timestamp.Equals(y.Timestamp) ? 0 : 1));
        }
    }
}
