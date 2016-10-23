using System;
using System.Collections.Generic;
using Domain;
using Domain.MissingValuePolicy;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    [TestClass]
    public abstract class GenericTestBase<T> : TestsBase
    {
        protected virtual void GivenASignal(Granularity granularity)
        {
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
        }

        public void WhenReadingData(DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            whenReadingDataResult = ClientGetData(signalId, fromIncludedUtc, toExcludedUtc).ToDomain<Domain.Datum<T>[]>();
        }

        public void WhenReadingCoarseData(Granularity coarseGranularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var coarseGranularityDto = coarseGranularity.ToDto<Dto.Granularity>();
            whenReadingDataResult = client.GetCoarseData(signalId, coarseGranularityDto, fromIncludedUtc, toExcludedUtc).ToDomain<Domain.Datum<T>[]>();
        }

        public void ThenResultEquals(IEnumerable<Datum<T>> expected)
        {
            Assertions.AreEqual(expected, whenReadingDataResult);
        }

        protected void WithMissingValuePolicy(MissingValuePolicyBase missingValuePolicy)
        {
            client.SetMissingValuePolicy(signalId, missingValuePolicy.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
        }

        public static T NoValue()
        {
            return default(T);
        }

        public static T Value(object value)
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
