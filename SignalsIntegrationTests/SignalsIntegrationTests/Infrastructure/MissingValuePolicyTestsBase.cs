using System;
using System.Collections.Generic;
using Domain;
using Domain.MissingValuePolicy;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assertions.AreEqual(expected, whenReadingDataResult);
        }

        protected IEnumerable<Datum<int>> whenReadingDataResult;
    }
}
