using System;
using System.Collections.Generic;
using Domain;
using Domain.MissingValuePolicy;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    [TestClass]
    public abstract class MissingValuePolicyTestsBase<T> : TestsBase
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
            whenReadingDataResult = client.GetData(signalId, fromIncludedUtc, toExcludedUtc).ToDomain<Domain.Datum<T>[]>();
        }

        public void ThenResultEquals(IEnumerable<Datum<T>> expected)
        {
            Assertions.AreEqual(expected, whenReadingDataResult);
        }

        protected T Value(int value)
        {
           return (T)Convert.ChangeType(value, typeof(T));
        }

        protected IEnumerable<Datum<T>> whenReadingDataResult;
    }
}
