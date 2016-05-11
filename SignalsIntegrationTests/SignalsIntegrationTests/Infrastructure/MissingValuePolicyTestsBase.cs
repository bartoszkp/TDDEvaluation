using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain;
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

        private class DatumEqualityComparer<T> : IComparer, IComparer<Datum<T>>
        {
            public int Compare(object x, object y)
            {
                var lhs = x as Datum<T>;
                var rhs = y as Datum<T>;
                if (lhs == null || rhs == null) throw new InvalidOperationException();
                return Compare(lhs, rhs);
            }

            public int Compare(Datum<T> x, Datum<T> y)
            {
                if (x.Value.Equals(y.Value) && x.Timestamp.Equals(y.Timestamp))
                    return 0;
                return 1;
            }
        }

        private void AssertDatumsEqual<T>(IEnumerable<Datum<T>> expected, IEnumerable<Datum<T>> actual)
        {
            CollectionAssert.AreEqual(expected.ToList(), actual.ToList(), new DatumEqualityComparer<T>());
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

            public void WithoutSignalDataExpect(Datum<int>[] expected)
            {
                CheckMissingValuePolicyBehavior(BuildEmptyInput(), expected);
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

            public Datum<int>[] BuildEmptyInput()
            {
                return new Datum<int>[0];
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

                parent.AssertDatumsEqual(expected, result.ToDomain<Domain.Datum<int>[]>());
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
