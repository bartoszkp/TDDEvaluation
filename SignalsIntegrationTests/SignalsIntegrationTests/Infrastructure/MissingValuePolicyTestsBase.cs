using Domain;
using Domain.MissingValuePolicy;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalsIntegrationTests.Infrastructure
{
    [TestClass]
    public abstract class MissingValuePolicyTestsBase : TestsBase
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            TestsBase.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            TestsBase.ClassCleanup();
        }

        protected void GivenASignal(Granularity granularity)
        {
            signalId = AddNewIntegerSignal(granularity).Id.Value;
        }

        protected void GivenNoData()
        {
            client.SetData(signalId, new Datum<int>[0].ToDto<Dto.Datum[]>());
        }

        protected void GivenSingleDatum(Datum<int> datum)
        {
            client.SetData(signalId, new[] { datum }.ToDto<Dto.Datum[]>());
        }

        protected void WithMissingValuePolicy(MissingValuePolicy missingValuePolicy)
        {
            client.SetMissingValuePolicy(signalId, missingValuePolicy.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
        }

        protected Datum<int>[] DatumWithNoneQualityFor(DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            return new Domain.Infrastructure.TimeEnumerator(fromIncludedUtc, toExcludedUtc, granularity)
                .Select(ts => new Datum<int>() { Quality = Quality.None, Timestamp = ts })
                .ToArray();
        }

        protected Datum<int>[] DatumWithSingleValueFollowedByNoneQualityFor(int value, DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            return Enumerable.Concat(
                new[] { new Datum<int>() { Quality = Quality.Good, Value = value, Timestamp = fromIncludedUtc } },
                new Domain.Infrastructure.TimeEnumerator(fromIncludedUtc, toExcludedUtc, granularity)
                    .Skip(1)
                    .Select(ts => new Datum<int>() { Quality = Quality.None, Timestamp = ts }))
                .ToArray();
        }

        protected Datum<int>[] DatumWithSingleValuePrecededByNoneQualityFor(int value, DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            return Enumerable.Concat(
                new Domain.Infrastructure.TimeEnumerator(fromIncludedUtc, toExcludedUtc.AddDays(-1), granularity)
                    .Select(ts => new Datum<int>() { Quality = Quality.None, Timestamp = ts }),
                new[] { new Datum<int>() { Quality = Quality.Good, Value = value, Timestamp = toExcludedUtc.AddDays(-1) } })
                .ToArray();
        }

        public void WhenReadingData(DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            whenReadingDataResult = client.GetData(signalId, fromIncludedUtc, toExcludedUtc).ToDomain<Domain.Datum<int>[]>();
        }

        public void ThenResultEquals(IEnumerable<Datum<int>> expected)
        {
            Assertions.AssertEqual(expected, whenReadingDataResult);
        }

        protected int signalId;
        protected IEnumerable<Datum<int>> whenReadingDataResult;

        /* TODO bad timestamps in GetData
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year
*/

        /* TODO correct timestamps in GetData (?)
                Second,
                Minute, 
                Hour,
                Day,    
                Week,
                Month,
                Year
        */

        /* TODO correct timestamps in SetData (?)
                    Second,
                    Minute,
                    Hour,
                    Day,
                    Week,
                    Month,
                    Year
        */

        // TODO SetMissing.... validates Params (?)
        // TODO GetData range validation

        // TODO GetData with different MissingValuePolicy

        // TODO removing?
        // TODO editing?
        // TODO changing path?

        // TODO persistency tests - problem - sequential run of unit tests...
    }
}
