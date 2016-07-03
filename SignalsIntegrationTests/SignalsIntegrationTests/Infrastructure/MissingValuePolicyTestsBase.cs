using Domain;
using Domain.MissingValuePolicy;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            GivenData();
        }

        protected void GivenSingleDatum(Datum<int> datum)
        {
            GivenData(datum);
        }

        protected void GivenData(params Datum<int>[] datums)
        {
            client.SetData(signalId, datums.ToDto<Dto.Datum[]>());
        }

        protected void WithMissingValuePolicy(MissingValuePolicyBase missingValuePolicy)
        {
            client.SetMissingValuePolicy(signalId, missingValuePolicy.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
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
