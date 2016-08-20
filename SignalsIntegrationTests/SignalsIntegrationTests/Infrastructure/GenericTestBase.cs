using System;
using System.Collections.Generic;
using Domain;
using Dto.Conversions;

namespace SignalsIntegrationTests.Infrastructure
{
    public class GenericTestBase<T> : TestsBase
    {
        public void WhenReadingData(DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            whenReadingDataResult = client.GetData(signalId, fromIncludedUtc, toExcludedUtc).ToDomain<Domain.Datum<T>[]>();
        }

        public void ThenResultEquals(IEnumerable<Datum<T>> expected)
        {
            Assertions.AreEqual(expected, whenReadingDataResult);
        }

        protected T Value(object value)
        {
            if (typeof(T).Equals(typeof(bool)))
            {
                return (T)Convert.ChangeType((int)value % 2 == 0, typeof(bool));
            }
            else if (typeof(T).Equals(typeof(string)))
            {
                return (T)Convert.ChangeType(value.ToString(), typeof(string));
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        protected IEnumerable<Datum<T>> whenReadingDataResult;
    }
}
