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

        public int SignalId { get; set; }

        protected void GivenASignal(Granularity granularity)
        {
            SignalId = AddNewIntegerSignal(granularity).Id.Value;
        }

        protected void WithNoData()
        {
            client.SetData(SignalId, new Datum<int>[0].ToDto<Dto.Datum[]>());
        }

        protected void WithMissingValuePolicy(MissingValuePolicy missingValuePolicy)
        {
            client.SetMissingValuePolicy(SignalId, missingValuePolicy.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
        }

        protected Datum<int>[] DatumWithNoneQualityFor(DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            return new Domain.Infrastructure.TimeEnumerator(fromIncludedUtc, toExcludedUtc, granularity)
                .Select(ts => new Datum<int> { Quality = Quality.None, Timestamp = ts })
                .ToArray();
        }

        public IEnumerable<Datum<int>> WhenReadingData(DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return client.GetData(SignalId, fromIncludedUtc, toExcludedUtc).ToDomain<Domain.Datum<int>[]>();
        }

        protected class MissingValuePolicyValidator
        {
            public Domain.MissingValuePolicy.MissingValuePolicy Policy { get; set; }
            public DateTime BeginTimestamp { get { return new DateTime(2020, 10, 12); } }
            public DateTime EndTimestamp { get { return BeginTimestamp.AddDays(5); } }
            public DateTime MiddleTimestamp { get { return BeginTimestamp.AddDays(2); } }
            public int GeneratedSingleValue { get { return 42; } }

            private MissingValuePolicyTestsBase parent;

            public MissingValuePolicyValidator(MissingValuePolicyTestsBase parent)
            {
                this.parent = parent;
            }

            public void WithSingleDatumAtBeginOfRangeExpect(Datum<int>[] expected)
            {
                CheckMissingValuePolicyBehavior(BuildSingleValueInput(BeginTimestamp), expected);
            }

            public void WithSingleDatumBeforeBeginOfRangeExpect(Datum<int>[] expected)
            {
                CheckMissingValuePolicyBehavior(BuildSingleValueInput(BeginTimestamp.AddDays(-1)), expected);
            }

            public void WithSingleDatumAtEndOfRangeExpect(Datum<int>[] expected)
            {
                CheckMissingValuePolicyBehavior(BuildSingleValueInput(EndTimestamp.AddDays(-1)), expected);
            }

            public void WithSingleDatumAfterEndOfRangeExpect(Datum<int>[] expected)
            {
                CheckMissingValuePolicyBehavior(BuildSingleValueInput(EndTimestamp), expected);
            }

            public void WithSingleDatumInMiddleOfRangeExpect(Datum<int>[] expected)
            {
                CheckMissingValuePolicyBehavior(BuildSingleValueInput(MiddleTimestamp), expected);
            }

            public Datum<int>[] BuildSingleValueInput(DateTime when)
            {
                return new[]
                {
                    new Datum<int>
                    {
                        Timestamp = when,
                        Value = GeneratedSingleValue,
                        Quality = Quality.Good
                    }
                };
            }

            public void CheckMissingValuePolicyBehavior(IEnumerable<Datum<int>> input,
                                                        IEnumerable<Datum<int>> expected)
            {
                var signalId = parent.AddNewIntegerSignal(Granularity.Day).Id.Value;
                parent.client.SetMissingValuePolicy(signalId, Policy.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
                
                parent.client.SetData(signalId, input.ToDto<Dto.Datum[]>());
                var result = parent.client.GetData(signalId, BeginTimestamp, EndTimestamp);

                Then.AssertEqual(expected, result.ToDomain<Domain.Datum<int>[]>());
            }
        }

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
