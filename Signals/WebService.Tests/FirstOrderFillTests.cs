using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using System.Collections.Generic;
using Domain.Services.Implementation;
using Moq;
using Domain.Repositories;
using System.Linq;

namespace WebService.Tests
{
    [TestClass]
    public class FirstOrderFillTests
    {
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenAnIntegerSignal_WhenGettingDataWithOneElement_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupWebService();

            int id = 1;

            var returnedSignal = new Signal() { Id = id, DataType = DataType.Integer };

            Mock<Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<int>> firstOrderPolicyMock = new Mock<Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<int>>();

            signalsRepoMock.Setup(sr => sr.Get(id)).Returns(returnedSignal);
            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(firstOrderPolicyMock.Object);
               

            dataRepoMock.Setup(d => d.GetData<int>(returnedSignal, new DateTime(2000, 11, 11), new DateTime(2000, 11, 11)))
                .Returns(new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.None, Timestamp = new DateTime(2000, 11, 11), Value = (int)1 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 11, 11), new DateTime(2000, 11, 11));

            var filledDatum = result.ElementAt(0);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Dto.Quality.None, filledDatum.Quality);
            Assert.AreEqual(0, filledDatum.Value);
        }

        [TestMethod]
        public void GivenAnIntegerSecondSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Second,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 13), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 9), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 12), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 13));

            var expectedDatum = GetExpectedDatums(Granularity.Second);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMinuteSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Minute,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 13, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 5, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 9, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 12, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 13, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Minute);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerHourSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Hour,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 13, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 9, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 12, 0, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 13, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Hour);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerDailySignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Day,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 14, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 6, 0, 0, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 13, 0, 0, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 14, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Day);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerDailySignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData_ForIssue31()
        {
            SetupFirstOrderPolicyFroSpecificExample(Granularity.Day, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 4, 0, 0, 0), new List<Datum<int>>()
            {
                new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0), Value = (int)10 },
                new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (int)30 }
            });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4));

            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = (int)10 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3), Value = (int)17 },
            };

            int i = 0;

            Assert.AreEqual(3, result.Count());
            foreach (var actualData in result)
            {
                Assert.AreEqual(expectedDatum[i].Timestamp, actualData.Timestamp);
               
                i++;
            }
        }

        [TestMethod]
        public void GivenAnIntegerDailySignal_WhenGettingDataWithCorrectRange_LowerQualityShouldFillMissingData()
        {
            SetupFirstOrderPolicyForLowerQuality(Granularity.Day, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 6, 0, 0, 0), new List<Datum<int>>()
            {
                new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)10 },
                new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (int)30 }
            });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 6));

            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (int)10 },
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 2), Value = (int)15 },
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 3), Value = (int)20 },
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 4), Value = (int)25 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 5), Value = (int)30 },
            };

            int i = 0;

            Assert.AreEqual(5, result.Count());
            foreach (var actualData in result)
            {
                Assert.AreEqual(expectedDatum[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatum[i].Quality, actualData.Quality);

                i++;
            }
        }

        [TestMethod]
        public void GivenAnIntegerSecondSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData_ForIssue31()
        {
            SetupFirstOrderPolicyFroSpecificExample(Granularity.Second, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 4), new List<Datum<int>>()
            {
                new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2), Value = (int)10 },
                new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 5), Value = (int)30 }
            });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 4));

            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2), Value = (int)10 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3), Value = (int)17 },
            };

            int i = 0;

            Assert.AreEqual(3, result.Count());
            foreach (var actualData in result)
            {
                Assert.AreEqual(expectedDatum[i].Timestamp, actualData.Timestamp);
                i++;
            }
        }

        [TestMethod]
        public void GivenAnIntegerWeeklySignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Week,
                new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 4, 3, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 7, 0, 0, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 6, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 27, 0, 0, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 4, 3, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Week);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMonthlySignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Month,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2001, 2, 1, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 6, 1, 0, 0, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 10, 1, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2001, 1, 1, 0, 0, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2001, 2, 1, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Month);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMonthSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData_ForIssue31()
        {
            SetupFirstOrderPolicyFroSpecificExample(Granularity.Month, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 4, 1, 0, 0, 0), new List<Datum<int>>()
            {
                new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1, 0, 0, 0), Value = (int)10 },
                new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1, 0, 0, 0), Value = (int)30 }
            });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)10 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (int)17 },
            };

            int i = 0;

            Assert.AreEqual(3, result.Count());
            foreach (var actualData in result)
            {
                Assert.AreEqual(expectedDatum[i].Timestamp, actualData.Timestamp);

                i++;
            }
        }

        [TestMethod]
        public void GivenAnIntegerHourSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData_ForIssue31()
        {
            SetupFirstOrderPolicyFroSpecificExample(Granularity.Hour, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0), new List<Datum<int>>()
            {
                new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0), Value = (int)10 },
                new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)30 }
            });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0));

            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0), Value = (int)10 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0), Value = (int)17 },
            };

            int i = 0;

            Assert.AreEqual(3, result.Count());
            foreach (var actualData in result)
            {
                Assert.AreEqual(expectedDatum[i].Timestamp, actualData.Timestamp);

                i++;
            }
        }

        [TestMethod]
        public void GivenAnIntegerYearSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Year,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2013, 1, 1, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2005, 1, 1, 0, 0, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2009, 1, 1, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2012, 1, 1, 0, 0, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2013, 1, 1, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Year);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerYearSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData_ForIssue31()
        {
            SetupFirstOrderPolicyFroSpecificExample(Granularity.Year, new DateTime(2001, 1, 1), new DateTime(2004, 1, 1), new List<Datum<int>>()
            {
                new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2002, 1, 1), Value = (int)10 },
                new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2005, 1, 1), Value = (int)30 }
            });

            var result = signalsWebService.GetData(1, new DateTime(2001, 1, 1), new DateTime(2004, 1, 1));

            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2001, 1, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1), Value = (int)10 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2003, 1, 1), Value = (int)17 },
            };

            int i = 0;

            Assert.AreEqual(3, result.Count());
            foreach (var actualData in result)
            {
                Assert.AreEqual(expectedDatum[i].Timestamp, actualData.Timestamp);

                i++;
            }
        }



        private void SetupFirstOrderPolicy(Granularity granularity,
            DateTime fromIncluded, DateTime toExcluded, List<Datum<int>> actualToBeReturnedByMockDatums)
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Integer };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger()
                { Id = 1, Signal = returnedSignal });


            dataRepoMock.Setup(d => d.GetData<int>(returnedSignal, fromIncluded, toExcluded))
                .Returns(actualToBeReturnedByMockDatums);

            SetupGetDataOlderAndNewerThanForSignal(returnedSignal);
        }

        private void SetupFirstOrderPolicyFroSpecificExample(Granularity granularity,
            DateTime fromIncluded, DateTime toExcluded, List<Datum<int>> actualToBeReturnedByMockDatums)
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Integer };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger()
                { Id = 1, Signal = returnedSignal });


            dataRepoMock.Setup(d => d.GetData<int>(returnedSignal, fromIncluded, toExcluded))
                .Returns(actualToBeReturnedByMockDatums);

            SetupGetDataOlderAndNewerThanForSignalForSpecificExample(returnedSignal);
        }

        private void SetupFirstOrderPolicyForLowerQuality(Granularity granularity,
           DateTime fromIncluded, DateTime toExcluded, List<Datum<int>> actualToBeReturnedByMockDatums)
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Integer };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger()
                { Id = 1, Signal = returnedSignal });


            dataRepoMock.Setup(d => d.GetData<int>(returnedSignal, fromIncluded, toExcluded))
                .Returns(actualToBeReturnedByMockDatums);

            SetupGetDataOlderAndNewerThanForSignalForSpecificExample(returnedSignal);
        }

        private void SetupGetDataOlderAndNewerThanForSignalForSpecificExample(Signal returnedSignal)
        {
            DateTime leftBadDatumTimestamp;
            DateTime middleFirstBadDatumTimestamp;
            DateTime middleSecondBadDatumTimestamp;
            DateTime middleThirdBadDatumTimestamp;
            DateTime rightGoodDatumTimestamp;


            leftBadDatumTimestamp = new DateTime(2000, 1, 1, 0, 0, 0);
            middleFirstBadDatumTimestamp = new DateTime(2000, 1, 2, 0, 0, 0);
            middleSecondBadDatumTimestamp = new DateTime(2000, 1, 3, 0, 0, 0);
            middleThirdBadDatumTimestamp = new DateTime(2000, 1, 4, 0, 0, 0);
            rightGoodDatumTimestamp = new DateTime(2000, 1, 5, 0, 0, 0);

            dataRepoMock
                .Setup(d => d.GetDataOlderThan<int>(returnedSignal, middleFirstBadDatumTimestamp, 1))
                .Returns(new List<Datum<int>>()
                {
                            new Datum<int>() { Quality = Quality.Bad, Signal = returnedSignal, Timestamp = leftBadDatumTimestamp, Value = (int)10 }
                });

            dataRepoMock
                .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftBadDatumTimestamp, 1))
                .Returns(new List<Datum<int>>()
                {
                            new Datum<int>() { Quality = Quality.Bad, Signal = returnedSignal, Timestamp = middleFirstBadDatumTimestamp, Value = (int)15 }
                });

            dataRepoMock
                .Setup(d => d.GetDataOlderThan<int>(returnedSignal, middleSecondBadDatumTimestamp, 1))
                 .Returns(new List<Datum<int>>()
                {
                            new Datum<int>() { Quality = Quality.Bad, Signal = returnedSignal, Timestamp = middleFirstBadDatumTimestamp, Value = (int)15 }
                });

            dataRepoMock
                .Setup(d => d.GetDataNewerThan<int>(returnedSignal, middleFirstBadDatumTimestamp, 1))
                .Returns(new List<Datum<int>>()
                {
                            new Datum<int>() { Quality = Quality.Bad, Signal = returnedSignal, Timestamp = middleSecondBadDatumTimestamp, Value = (int)20 }
                });

            dataRepoMock
                .Setup(d => d.GetDataOlderThan<int>(returnedSignal, middleThirdBadDatumTimestamp, 1))
                 .Returns(new List<Datum<int>>()
                {
                            new Datum<int>() { Quality = Quality.Bad, Signal = returnedSignal, Timestamp = middleSecondBadDatumTimestamp, Value = (int)20 }
                });

            dataRepoMock
               .Setup(d => d.GetDataNewerThan<int>(returnedSignal, middleSecondBadDatumTimestamp, 1))
               .Returns(new List<Datum<int>>()
               {
                            new Datum<int>() { Quality = Quality.Bad, Signal = returnedSignal, Timestamp = middleThirdBadDatumTimestamp, Value = (int)25 }
               });

            dataRepoMock
                .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightGoodDatumTimestamp, 1))
                 .Returns(new List<Datum<int>>()
                {
                            new Datum<int>() { Quality = Quality.Bad, Signal = returnedSignal, Timestamp = middleThirdBadDatumTimestamp, Value = (int)25 }
                });

            dataRepoMock
               .Setup(d => d.GetDataNewerThan<int>(returnedSignal, middleThirdBadDatumTimestamp, 1))
               .Returns(new List<Datum<int>>()
               {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = rightGoodDatumTimestamp, Value = (int)30 }
               });
        }

        private void AssertEqual(List<Dto.Datum> expectedDatums, IEnumerable<Dto.Datum> actualDatums)
        {
            int i = 0;

            Assert.AreEqual(13, actualDatums.Count());
            foreach (var actualData in actualDatums)
            {
                Assert.AreEqual(expectedDatums[i].Quality, actualData.Quality);
                Assert.AreEqual(expectedDatums[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatums[i].Value, actualData.Value);

                i++;
            }
        }

        private void SetupWebService()
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private List<Dto.Datum> GetExpectedDatums(Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 2), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 6), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 7), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 8), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 9), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 10), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 11), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 12), Value = (int)28 },
                    };

                case Granularity.Minute:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 2, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 3, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 4, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 5, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 6, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 7, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 8, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 9, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 10,0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 11,0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 12,0), Value = (int)28 },
                    };

                case Granularity.Hour:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 4, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 6, 0, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 7, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 8, 0, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 9, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 10, 0, 0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 11, 0, 0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 12, 0, 0), Value = (int)28 },
                    };

                case Granularity.Day:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 4, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 6, 0, 0, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 7, 0, 0, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 8, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 9, 0, 0, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 11, 0, 0, 0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 12, 0, 0, 0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 13, 0, 0, 0), Value = (int)28 },
                    };

                case Granularity.Week:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 17, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 24, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 31, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 7, 0, 0, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 14, 0, 0, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 21, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 28, 0, 0, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 6, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 13, 0, 0, 0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 20, 0, 0, 0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 27, 0, 0, 0), Value = (int)28 },
                    };

                case Granularity.Month:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1, 0, 0, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 6, 1, 0, 0, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 7, 1, 0, 0, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 9, 1, 0, 0, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 10, 1, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 11, 1, 0, 0, 0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 12, 1, 0, 0, 0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2001, 1, 1, 0, 0, 0), Value = (int)28 },
                    };

                case Granularity.Year:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1, 0, 0, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2003, 1, 1, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2004, 1, 1, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 1, 1, 0, 0, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2006, 1, 1, 0, 0, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2007, 1, 1, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2008, 1, 1, 0, 0, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2009, 1, 1, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2010, 1, 1, 0, 0, 0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2011, 1, 1, 0, 0, 0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2012, 1, 1, 0, 0, 0), Value = (int)28 },
                    };
            }
            return null;
        }

        private void SetupGetDataOlderAndNewerThanForSignal(Signal returnedSignal)
        {
            switch (returnedSignal.Granularity)
            {
                case Granularity.Second:
                    var firstTimestamp = new DateTime(2000, 1, 1, 0, 0, 1);
                    var secondTimestamp = new DateTime(2000, 1, 1, 0, 0, 5);
                    var thirdTimestamp = new DateTime(2000, 1, 1, 0, 0, 9);
                    var fourthTimestamp = new DateTime(2000, 1, 1, 0, 0, 12);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestamp.AddSeconds(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestamp, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestamp, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestamp, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestamp, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestamp, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestamp, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestamp.AddSeconds(1), Value = default(int) }
                        });
                    break;

                case Granularity.Minute:
                    var firstTimestampMinute = new DateTime(2000, 1, 1, 0, 1, 0);
                    var secondTimestampMinute = new DateTime(2000, 1, 1, 0, 5, 0);
                    var thirdTimestampMinute = new DateTime(2000, 1, 1, 0, 9, 0);
                    var fourthTimestampMinute = new DateTime(2000, 1, 1, 0, 12, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampMinute, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampMinute.AddMinutes(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampMinute, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampMinute, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampMinute, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampMinute, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampMinute, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampMinute, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampMinute, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampMinute.AddMinutes(1), Value = default(int) }
                        });
                    break;

                case Granularity.Hour:
                    var firstTimestampHour = new DateTime(2000, 1, 1, 1, 0, 0);
                    var secondTimestampHour = new DateTime(2000, 1, 1, 5, 0, 0);
                    var thirdTimestampHour = new DateTime(2000, 1, 1, 9, 0, 0);
                    var fourthTimestampHour = new DateTime(2000, 1, 1, 12, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampHour, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampHour.AddHours(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampHour, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampHour, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampHour, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampHour, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampHour, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampHour, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampHour, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampHour.AddHours(1), Value = default(int) }
                        });
                    break;

                case Granularity.Day:
                    var firstTimestampDay = new DateTime(2000, 1, 2, 0, 0, 0);
                    var secondTimestampDay = new DateTime(2000, 1, 6, 0, 0, 0);
                    var thirdTimestampDay = new DateTime(2000, 1, 10, 0, 0, 0);
                    var fourthTimestampDay = new DateTime(2000, 1, 13, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampDay, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampDay.AddDays(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampDay, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampDay, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampDay, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampDay, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampDay, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampDay, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampDay, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampDay.AddDays(1), Value = default(int) }
                        });
                    break;

                case Granularity.Week:
                    var firstTimestampWeek = new DateTime(2000, 1, 10, 0, 0, 0);
                    var secondTimestampWeek = new DateTime(2000, 2, 7, 0, 0, 0);
                    var thirdTimestampWeek = new DateTime(2000, 3, 6, 0, 0, 0);
                    var fourthTimestampWeek = new DateTime(2000, 3, 27, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampWeek, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampWeek.AddDays(-7), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampWeek, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampWeek, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampWeek.AddDays(7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampWeek, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampWeek.AddDays(7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampWeek, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampWeek.AddDays(7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampWeek, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampWeek.AddDays(7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampWeek, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampWeek.AddDays(7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampWeek, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampWeek.AddDays(7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampWeek.AddDays(7), Value = default(int) }
                        });
                    break;

                case Granularity.Month:
                    var firstTimestampMonth = new DateTime(2000, 2, 1, 0, 0, 0);
                    var secondTimestampMonth = new DateTime(2000, 6, 1, 0, 0, 0);
                    var thirdTimestampMonth = new DateTime(2000, 10, 1, 0, 0, 0);
                    var fourthTimestampMonth  = new DateTime(2001, 1, 1, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampMonth, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampMonth.AddMonths(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampMonth, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampMonth, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampMonth.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampMonth, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampMonth.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampMonth, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampMonth.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampMonth, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampMonth.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampMonth, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampMonth.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampMonth, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampMonth.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampMonth.AddMonths(1), Value = default(int) }
                        });
                    break;

                case Granularity.Year:
                    var firstTimestampYear = new DateTime(2001, 1, 1, 0, 0, 0);
                    var secondTimestampYear = new DateTime(2005, 1, 1, 0, 0, 0);
                    var thirdTimestampYear = new DateTime(2009, 1, 1, 0, 0, 0);
                    var fourthTimestampYear = new DateTime(2012, 1, 1, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampYear, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampYear.AddYears(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampYear, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampYear, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampYear.AddYears(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampYear, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampYear.AddYears(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampYear, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampYear.AddYears(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampYear, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampYear.AddYears(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampYear, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampYear.AddYears(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampYear, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampYear.AddYears(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampYear.AddYears(1), Value = default(int) }
                        });
                    break;
            }
        }
    }

    [TestClass]
    public class FirstOrderFillsTestsWithTimeStampOutOfRange
    {
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenAnIntegerSecondSignal_WhenGettingDataWithTimeStampLeftAndRightOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Second,
                new DateTime(2000, 1, 1, 0, 0, 3), new DateTime(2000, 1, 1, 0, 0, 9), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 6), Value = (int)6 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 3), new DateTime(2000, 1, 1, 0, 0, 9));

            var expectedDatum = GetExpectedDatums(Granularity.Second);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMinuteSignal_WhenGettingDataWithTimeStampLeftAndRightOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Minute,
                new DateTime(2000, 1, 1, 0, 3, 0), new DateTime(2000, 1, 1, 0, 9, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 5, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 6, 0), Value = (int)6 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 3, 0), new DateTime(2000, 1, 1, 0, 9, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Minute);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerHourSignal_WhenGettingDataWithTimeStampLeftAndRightOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Hour,
                new DateTime(2000, 1, 1, 3, 0, 0), new DateTime(2000, 1, 1, 9, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 6, 0, 0), Value = (int)6 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 3, 0, 0), new DateTime(2000, 1, 1, 9, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Hour);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerDailySignal_WhenGettingDataWithTimeStampLeftAndRightOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Day,
                new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 1, 9, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 6, 0, 0, 0), Value = (int)6 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 1, 9, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Day);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerWeekSignal_WhenGettingDataWithTimeStampLeftAndRightOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Week,
                new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 2, 14, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 17, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 24, 0, 0, 0), Value = (int)6 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 2, 14, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Week);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMonthlySignal_WhenGettingDataWithTimeStampLeftAndRightOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Month,
                new DateTime(2000, 3, 1, 0, 0, 0), new DateTime(2000, 9, 1, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 1, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 6, 1, 0, 0, 0), Value = (int)6 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 3, 1, 0, 0, 0), new DateTime(2000, 9, 1, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Month);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerYearlySignal_WhenGettingDataWithTimeStampLeftAndRightOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Year,
                new DateTime(2003, 1, 1, 0, 0, 0), new DateTime(2009, 1, 1, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2005, 1, 1, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2006, 1, 1, 0, 0, 0), Value = (int)6 },
                });

            var result = signalsWebService.GetData(1, new DateTime(2003, 1, 1, 0, 0, 0), new DateTime(2009, 1, 1, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Year);

            AssertEqual(expectedDatum, result);
        }

        private void SetupFirstOrderPolicy(Granularity granularity,
            DateTime fromIncluded, DateTime toExcluded, List<Datum<int>> actualToBeReturnedByMockDatums)
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Integer };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger()
                { Id = 1, Signal = returnedSignal });

            dataRepoMock.Setup(d => d.GetData<int>(returnedSignal, fromIncluded, toExcluded))
                .Returns(actualToBeReturnedByMockDatums);

            SetupGetDataOlderAndNewerThanForSignal(returnedSignal);
        }

        private void AssertEqual(List<Dto.Datum> expectedDatums, IEnumerable<Dto.Datum> actualDatums)
        {
            int i = 0;

            Assert.AreEqual(expectedDatums.Count(), actualDatums.Count());
            foreach (var actualData in actualDatums)
            {
                Assert.AreEqual(expectedDatums[i].Quality, actualData.Quality);
                Assert.AreEqual(expectedDatums[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatums[i].Value, actualData.Value);

                i++;
            }
        }

        private List<Dto.Datum> GetExpectedDatums(Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 6), Value = (int)6 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 0, 7), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 0, 8), Value = default(int) },
                    };

                case Granularity.Minute:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 3, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 4, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 5, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 6, 0), Value = (int)6 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 7, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 0, 8, 0), Value = default(int) },
                    };

                case Granularity.Hour:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 4, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 6, 0, 0), Value = (int)6 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 7, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 8, 0, 0), Value = default(int) },
                    };

                case Granularity.Day:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 4, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 6, 0, 0, 0), Value = (int)6 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 7, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 8, 0, 0, 0), Value = default(int) },
                    };

                case Granularity.Week:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 17, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 24, 0, 0, 0), Value = (int)6 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 31, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 7, 0, 0, 0), Value = default(int) },
                    };

                case Granularity.Month:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 6, 1, 0, 0, 0), Value = (int)6 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 8, 1, 0, 0, 0), Value = default(int) },
                    };

                case Granularity.Year:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2003, 1, 1, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2004, 1, 1, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2006, 1, 1, 0, 0, 0), Value = (int)6 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2007, 1, 1, 0, 0, 0), Value = default(int) },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2008, 1, 1, 0, 0, 0), Value = default(int) },
                    };
            }

            return null;
        }

        private void SetupGetDataOlderAndNewerThanForSignal(Signal returnedSignal)
        {
            DateTime leftNoneDatumTimestamp;
            DateTime middleFairDatumTimestamp;
            DateTime middleGoodDatumTimestamp;
            DateTime rightNoneDatumTimestamp;

            switch (returnedSignal.Granularity)
            { 
                case Granularity.Second:
                    leftNoneDatumTimestamp = new DateTime(2000, 1, 1, 0, 0, 3);
                    middleFairDatumTimestamp = new DateTime(2000, 1, 1, 0, 0, 5);
                    middleGoodDatumTimestamp = new DateTime(2000, 1, 1, 0, 0, 6);
                    rightNoneDatumTimestamp = new DateTime(2000, 1, 1, 0, 0, 8);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp.AddSeconds(-1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp.AddSeconds(-1), 1))
                        .Returns(new List<Datum<int>>());


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    break;

                case Granularity.Minute:
                    leftNoneDatumTimestamp = new DateTime(2000, 1, 1, 0, 3, 0);
                    middleFairDatumTimestamp = new DateTime(2000, 1, 1, 0, 5, 0);
                    middleGoodDatumTimestamp = new DateTime(2000, 1, 1, 0, 6, 0);
                    rightNoneDatumTimestamp = new DateTime(2000, 1, 1, 0, 8, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp.AddMinutes(-1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp.AddMinutes(-1), 1))
                        .Returns(new List<Datum<int>>());


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    break;

                case Granularity.Hour:
                    leftNoneDatumTimestamp = new DateTime(2000, 1, 1, 3, 0, 0);
                    middleFairDatumTimestamp = new DateTime(2000, 1, 1, 5, 0, 0);
                    middleGoodDatumTimestamp = new DateTime(2000, 1, 1, 6, 0, 0);
                    rightNoneDatumTimestamp = new DateTime(2000, 1, 1, 8, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp.AddHours(1), 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp.AddHours(-1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp.AddHours(-1), 1))
                        .Returns(new List<Datum<int>>());


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    break;

                case Granularity.Day:
                    leftNoneDatumTimestamp = new DateTime(2000, 1, 3, 0, 0, 0);
                    middleFairDatumTimestamp = new DateTime(2000, 1, 5, 0, 0, 0);
                    middleGoodDatumTimestamp = new DateTime(2000, 1, 6, 0, 0, 0);
                    rightNoneDatumTimestamp = new DateTime(2000, 1, 8, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp.AddDays(1), 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp.AddDays(-1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp.AddDays(-1), 1))
                        .Returns(new List<Datum<int>>());


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    break;

                case Granularity.Week:
                    leftNoneDatumTimestamp = new DateTime(2000, 1, 3, 0, 0, 0);
                    middleFairDatumTimestamp = new DateTime(2000, 1, 17, 0, 0, 0);
                    middleGoodDatumTimestamp = new DateTime(2000, 1, 24, 0, 0, 0);
                    rightNoneDatumTimestamp = new DateTime(2000, 2, 7, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp.AddDays(7), 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp.AddDays(7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp.AddDays(-7), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp.AddDays(-7), 1))
                        .Returns(new List<Datum<int>>());


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    break;

                case Granularity.Month :
                    leftNoneDatumTimestamp = new DateTime(2000, 3, 1, 0, 0, 0);
                    middleFairDatumTimestamp = new DateTime(2000, 5, 1, 0, 0, 0);
                    middleGoodDatumTimestamp = new DateTime(2000, 6, 1, 0, 0, 0);
                    rightNoneDatumTimestamp = new DateTime(2000, 8, 1, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp.AddMonths(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp.AddMonths(-1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp.AddMonths(-1), 1))
                        .Returns(new List<Datum<int>>());


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    break;

                case Granularity.Year:
                    leftNoneDatumTimestamp = new DateTime(2003, 1, 1, 0, 0, 0);
                    middleFairDatumTimestamp = new DateTime(2005, 1, 1, 0, 0, 0);
                    middleGoodDatumTimestamp = new DateTime(2006, 1, 1, 0, 0, 0);
                    rightNoneDatumTimestamp = new DateTime(2008, 1, 1, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, leftNoneDatumTimestamp.AddYears(1), 1))
                        .Returns(new List<Datum<int>>());

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, leftNoneDatumTimestamp.AddYears(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = middleFairDatumTimestamp, Value = (int)5 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp.AddYears(-1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp.AddYears(-1), 1))
                        .Returns(new List<Datum<int>>());


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = middleGoodDatumTimestamp, Value = (int)6 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, rightNoneDatumTimestamp, 1))
                        .Returns(new List<Datum<int>>());

                    break;
            }
        }

        [TestMethod]
        public void GivenAnIntegerSecondSignal_WhenGettingDataWithTimeStampMiddleOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            DateTime fromIncluded = new DateTime(2000, 1, 1, 0, 0, 4);
            DateTime toExcluded = new DateTime(2000, 1, 1, 0, 0, 5);
            var granularity = Granularity.Second;

            SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(granularity, fromIncluded, toExcluded);

            var result = signalsWebService.GetData(1, fromIncluded, toExcluded);

            var expectedDatum = GetExpectedSingleDatum(granularity);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMinuteSignal_WhenGettingDataWithTimeStampMiddleOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            DateTime fromIncluded = new DateTime(2000, 1, 1, 0, 4, 0);
            DateTime toExcluded = new DateTime(2000, 1, 1, 0, 5, 0);
            var granularity = Granularity.Minute;

            SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(granularity, fromIncluded, toExcluded);

            var result = signalsWebService.GetData(1, fromIncluded, toExcluded);

            var expectedDatum = GetExpectedSingleDatum(granularity);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerHourSignal_WhenGettingDataWithTimeStampMiddleOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            DateTime fromIncluded = new DateTime(2000, 1, 1, 4, 0, 0);
            DateTime toExcluded = new DateTime(2000, 1, 1, 5, 0, 0);
            var granularity = Granularity.Hour;

            SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(granularity, fromIncluded, toExcluded);

            var result = signalsWebService.GetData(1, fromIncluded, toExcluded);

            var expectedDatum = GetExpectedSingleDatum(granularity);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerDailySignal_WhenGettingDataWithTimeStampMiddleOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            DateTime fromIncluded = new DateTime(2000, 1, 4, 0, 0, 0);
            DateTime toExcluded = new DateTime(2000, 1, 5, 0, 0, 0);
            var granularity = Granularity.Day;

            SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(granularity, fromIncluded, toExcluded);

            var result = signalsWebService.GetData(1, fromIncluded, toExcluded);

            var expectedDatum = GetExpectedSingleDatum(granularity);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerWeekSignal_WhenGettingDataWithTimeStampMiddleOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            DateTime fromIncluded = new DateTime(2000, 1, 10, 0, 0, 0);
            DateTime toExcluded = new DateTime(2000, 1, 17, 0, 0, 0);
            var granularity = Granularity.Week;

            SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(granularity, fromIncluded, toExcluded);

            var result = signalsWebService.GetData(1, fromIncluded, toExcluded);

            var expectedDatum = GetExpectedSingleDatum(granularity);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMontlySignal_WhenGettingDataWithTimeStampMiddleOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            DateTime fromIncluded = new DateTime(2000, 4, 1, 0, 0, 0);
            DateTime toExcluded = new DateTime(2000, 5, 1, 0, 0, 0);
            var granularity = Granularity.Month;

            SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(granularity, fromIncluded, toExcluded);

            var result = signalsWebService.GetData(1, fromIncluded, toExcluded);

            var expectedDatum = GetExpectedSingleDatum(granularity);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerYearlySignal_WhenGettingDataWithTimeStampMiddleOutOfRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            DateTime fromIncluded = new DateTime(2004, 1, 1, 0, 0, 0);
            DateTime toExcluded = new DateTime(2005, 1, 1, 0, 0, 0);
            var granularity = Granularity.Year;

            SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(granularity, fromIncluded, toExcluded);

            var result = signalsWebService.GetData(1, fromIncluded, toExcluded);

            var expectedDatum = GetExpectedSingleDatum(granularity);

            AssertEqual(expectedDatum, result);
        }

        private void SetupFirstOrderPolicyForTimeStampMiddleOutOfRange(Granularity granularity, DateTime fromIncluded, DateTime toExcluded)
        {
            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Integer };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger()
                { Id = 1, Signal = returnedSignal });

            dataRepoMock
                .Setup(d => d.GetData<int>(returnedSignal, fromIncluded, toExcluded))
                .Returns(new List<Datum<int>>());

            SetupDataRepoForSignleDatum(returnedSignal);

            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private List<Dto.Datum> GetExpectedSingleDatum(Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = 4 }, };

                case Granularity.Minute:
                    return new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 4, 0), Value = 4 }, };

                case Granularity.Hour:
                    return new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 4, 0, 0), Value = 4 }, };

                case Granularity.Day:
                    return new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 4, 0, 0, 0), Value = 4 }, };

                case Granularity.Week:
                    return new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = 4 }, };

                case Granularity.Month:
                    return new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 4, 1, 0, 0, 0), Value = 4 }, };

                case Granularity.Year:
                    return new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2004, 1, 1, 0, 0, 0), Value = 4 }, };
            }
            return null;
        }

        private void SetupWebService()
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private void SetupDataRepoForSignleDatum(Signal returnedSignal)
        {
            switch (returnedSignal.Granularity)
            {
                case Granularity.Second:
                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, new DateTime(2000, 1, 1, 0, 0, 4), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = 3 }
                        });
                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, new DateTime(2000, 1, 1, 0, 0, 4), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = 5 }
                        });
                    break;

                case Granularity.Minute:
                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, new DateTime(2000, 1, 1, 0, 4, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 1, 0, 3, 0), Value = 3 }
                        });
                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, new DateTime(2000, 1, 1, 0, 4, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 1, 0, 5, 0), Value = 5 }
                        });
                    break;

                case Granularity.Hour:
                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, new DateTime(2000, 1, 1, 4, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0), Value = 3 }
                        });
                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, new DateTime(2000, 1, 1, 4, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = 5 }
                        });
                    break;

                case Granularity.Day:
                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, new DateTime(2000, 1, 4, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = 3 }
                        });
                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, new DateTime(2000, 1, 4, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = 5 }
                        });
                    break;

                case Granularity.Week:
                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, new DateTime(2000, 1, 10, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = 3 }
                        });
                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, new DateTime(2000, 1, 10, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = new DateTime(2000, 1, 17, 0, 0, 0), Value = 5 }
                        });
                    break;

                case Granularity.Month:
                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, new DateTime(2000, 4, 1, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = new DateTime(2000, 3, 1, 0, 0, 0), Value = 3 }
                        });
                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, new DateTime(2000, 4, 1, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = new DateTime(2000, 5, 1, 0, 0, 0), Value = 5 }
                        });
                    break;

                case Granularity.Year:
                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, new DateTime(2004, 1, 1, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = new DateTime(2003, 1, 1, 0, 0, 0), Value = 3 }
                        });
                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, new DateTime(2004, 1, 1, 0, 0, 0), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = new DateTime(2005, 1, 1, 0, 0, 0), Value = 5 }
                        });
                    break;
            }
        }
    }
}
