using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class Assertions
    {
        public static void AssertThrows(Action f)
        {
            try
            {
                f();
                Assert.Fail();
            }
            catch (FaultException e)
            {
                AssertNotBuiltInException(e);
            }
        }

        private static void AssertNotBuiltInException(FaultException e)
        {
            var ex = e as FaultException<ExceptionDetail>;
            Assert.AreNotEqual(typeof(NotImplementedException).ToString(), ex?.Detail.Type);
            Assert.AreNotEqual(typeof(InvalidCastException).ToString(), ex?.Detail.Type);
        }

        public static void AreEqual<T>(Domain.Datum<T> expected, Domain.Datum<T> actual)
        {
            AreEqual(new[] { expected }, new[] { actual });
        }

        public static void AreEqual<T>(IEnumerable<Domain.Datum<T>> expected, IEnumerable<Domain.Datum<T>> actual)
        {
            CollectionAssert.AreEqual(
                expected.ToList(),
                actual.ToList(),
                Comparer<Domain.Datum<T>>.Create((x, y) => x.Quality.Equals(y.Quality)
                                                            && x.Timestamp == y.Timestamp
                                                            && ((x.Value == null && y.Value == null) || x.Value.Equals(y.Value)) ? 0 : 1));
        }

        public static void AreEqual(Dto.Datum expected, Dto.Datum actual, string message = "")
        {
            AreEqual(new[] { expected }, new[] { actual }, message);
        }

        public static void AreEqual(IEnumerable<Dto.Datum> expected, IEnumerable<Dto.Datum> actual, string message = "")
        {
            CollectionAssert.AreEqual(
                expected.ToList(),
                actual.ToList(),
                Comparer<Dto.Datum>.Create((x, y) => x.Quality.Equals(y.Quality)
                                                     && x.Timestamp.Equals(y.Timestamp)
                                                     && x.Value.Equals(y.Value) ? 0 : 1),
                message);
        }
    }
}
