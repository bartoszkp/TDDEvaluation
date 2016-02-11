﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Dto.Conversions;
using SignalsIntegrationTests.Infrastructure;
using System;
using System.Threading;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class SignalTests : TestsBase
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

        [TestMethod]
        public void RequestForNonExistingSignalThrowsOrReturnsNull()
        {
            var path = Path.FromString("/non/existent/path");

            Assertions.AssertReturnsNullOrThrows(() => client.Get(path.ToDto<Dto.Path>()));
        }

        [TestMethod]
        public void AddingSignalSetsItsId()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            signal = client.Add(signal.ToDto<Dto.Signal>()).ToDomain<Domain.Signal>();

            Assert.IsNotNull(signal.Id);
        }

        [TestMethod]
        public void AddedSignalCanBeRetrieved()
        {
            var newSignal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            client.Add(newSignal.ToDto<Dto.Signal>());
            var received = client.Get(newSignal.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();

            Assert.AreEqual(newSignal.DataType, received.DataType);
            Assert.AreEqual(newSignal.Path, received.Path);
            Assert.AreEqual(received.Granularity, received.Granularity);
        }

        [TestMethod]
        public void MultipleSignalsCanBeStoredSimultanously()
        {
            var newSignal1 = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };
            var newSignal2 = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Hour,
                DataType = DataType.Double
            };

            client.Add(newSignal1.ToDto<Dto.Signal>());
            client.Add(newSignal2.ToDto<Dto.Signal>());
            var received1 = client.Get(newSignal1.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();
            var received2 = client.Get(newSignal2.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();

            Assert.AreEqual(newSignal1.Path, received1.Path);
            Assert.AreEqual(newSignal2.Path, received2.Path);
            Assert.AreNotEqual(received1.Id, received2.Id);
        }

        [TestMethod]
        public void CanWriteAndRetrieveData()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 4, 14);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal, timestamp, timestamp.AddDays(1));

            Assert.AreEqual(data.Length, retrievedData.Length);
            Assert.AreEqual(data[0].Value, retrievedData[0].Value);
            Assert.AreEqual(data[0].Timestamp, retrievedData[0].Timestamp);
            Assert.AreEqual(data[0].Quality, retrievedData[0].ToDomain<Domain.Datum<int>>().Quality);
        }

        [TestMethod]
        public void GetDataUsingIncompleteSignalsThrowsOrReturnsNull()
        {
            var newSignal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            Assertions.AssertReturnsNullOrThrows(() => client.GetData(newSignal.ToDto<Dto.Signal>(), new DateTime(2016, 12, 10), new DateTime(2016, 12, 14)));
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataUsingIncompleteSignalsThrows()
        {
            var timestamp = new DateTime(2019, 4, 14);
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };
            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        public void TryingToAddSignalWithExistingPathThrowsOrReturnsNull()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            client.Add(signal.ToDto<Dto.Signal>());

            Assertions.AssertReturnsNullOrThrows(() => client.Add(signal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        public void TryingToAddSignalWithNotNullIdThrowsOrReturnsNull()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer,
                Id = 42
            };

            Assertions.AssertReturnsNullOrThrows(() => client.Add(signal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        public void SignalWithoutDataReturnsNoneQualityDatumsForEachTimerangeStep()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            const int numberOfDays = 5;
            var timestamp = new DateTime(2019, 1, 1);
            var receivedData = client.GetData(signal, timestamp, timestamp.AddDays(numberOfDays));

            Assert.AreEqual(numberOfDays, receivedData.Length);
            foreach (var datum in receivedData)
            {
                Assert.AreEqual(timestamp, datum.Timestamp);
                Assert.AreEqual(Dto.Quality.None, datum.Quality);
                timestamp = timestamp.AddDays(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForSecondGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Second);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 44, 123),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMinuteGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Minute);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 123),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMinuteGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Minute);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForHourGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Hour);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForHourGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Hour);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForHourGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Hour);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForDayGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForDayGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForDayGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForDayGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresMondayInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }


        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            signal = client.Add(signal);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresFirstMonthInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 2, 1),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        public void NewSignalHasNoneQualityMissingValuePolicy()
        {
            var signal = AddNewIntegerSignal();

            var result = client.GetMissingValuePolicyConfig(signal);

            Assert.AreEqual(Dto.MissingValuePolicy.NoneQuality, result.Policy);
        }

        private Dto.Signal AddNewIntegerSignal(Domain.Granularity granularity = Granularity.Second)
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = granularity,
                DataType = DataType.Integer,
            };

            return client.Add(signal.ToDto<Dto.Signal>());
        }

        [TestMethod]
        public void MissingValuePolicyCanBeSetForSignal()
        {
            var signal = AddNewIntegerSignal();

            var newConfig = new MissingValuePolicyConfig() { Policy = MissingValuePolicy.SpecificValue };

            client.SetMissingValuePolicyConfig(signal, newConfig.ToDto<Dto.MissingValuePolicyConfig>());
            var result = client.GetMissingValuePolicyConfig(signal);

            Assert.AreEqual(newConfig.Policy, result.ToDomain<Domain.MissingValuePolicyConfig>().Policy);
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

        private class MissingValuePolicyValidator
        {
            public MissingValuePolicyConfig PolicyConfig { get; set; }
            public DateTime BeginTimestamp { get { return new DateTime(2020, 10, 12); } }
            public DateTime EndTimestamp { get { return BeginTimestamp.AddDays(5); } }
            public DateTime MiddleTimestamp { get { return BeginTimestamp.AddDays(2); } }
            public int GeneratedSingleValue { get { return 42; } }

            private SignalTests _parent;

            public MissingValuePolicyValidator(SignalTests parent)
            {
                _parent = parent;
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
                var signal = _parent.AddNewIntegerSignal(Granularity.Day);
                _parent.client.SetMissingValuePolicyConfig(signal, PolicyConfig.ToDto<Dto.MissingValuePolicyConfig>());

                _parent.client.SetData(signal, input.ToDto<Dto.Datum[]>());
                var result = _parent.client.GetData(signal, BeginTimestamp, EndTimestamp);

                _parent.AssertDatumsEqual(expected, result.ToDomain<Domain.Datum<int>[]>());
            }
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenNoDataPresent()
        {
            var validator = CreateNoneQualityPolicyValidator();
            validator.WithoutSignalDataExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAtBeginOfRangePresent()
        {
            var validator = CreateNoneQualityPolicyValidator();
            validator.WithSingleDatumAtBeginOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.Good, Timestamp = validator.BeginTimestamp, Value = validator.GeneratedSingleValue },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumBeforeBeginOfRangePresent()
        {
            var validator = CreateNoneQualityPolicyValidator();
            validator.WithSingleDatumBeforeBeginOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp,},
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAtEndOfRangePresent()
        {
            MissingValuePolicyValidator validator = CreateNoneQualityPolicyValidator();
            validator.WithSingleDatumAtEndOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.Good, Timestamp = validator.BeginTimestamp.AddDays(4), Value = validator.GeneratedSingleValue }
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAfterEndOfRangePresent()
        {
            MissingValuePolicyValidator validator = CreateNoneQualityPolicyValidator();
            validator.WithSingleDatumAfterEndOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumInMiddleRangePresent()
        {
            MissingValuePolicyValidator validator = CreateNoneQualityPolicyValidator();
            validator.WithSingleDatumInMiddleOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.Good, Timestamp = validator.BeginTimestamp.AddDays(2), Value = validator.GeneratedSingleValue },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        private MissingValuePolicyValidator CreateNoneQualityPolicyValidator()
        {
            return new MissingValuePolicyValidator(this)
            {
                PolicyConfig = new MissingValuePolicyConfig() { Policy = MissingValuePolicy.NoneQuality },
            };
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
