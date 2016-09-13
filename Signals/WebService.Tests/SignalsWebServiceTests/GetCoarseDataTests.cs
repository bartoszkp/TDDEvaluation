using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;
using DataAccess.GenericInstantiations;
using Dto.MissingValuePolicy;
using Moq;
using System.Reflection;
using System.Linq;
using Domain;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class GetCoarseDataTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
        public void GivenNoSignals_GetCoarseDataByIdAndTime_ExpectedException()
        {
            SetupGet();

            signalsWebService.GetCoarseData(1, Dto.Granularity.Month,new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
        }

        [TestMethod]
        public void GivenASignalWithNoDataSource_WhenGettingCoarseData_ChecksLengthOfArray()
        {
            var signalId = 1;
            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Day));
            SetupMVPGet(new NoneQualityMissingValuePolicyDouble());

            var result = signalsWebService.GetCoarseData(signalId,Dto.Granularity.Week ,new DateTime(2016, 9, 12), new DateTime(2016, 10, 3));

            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void GivenASignalWithDataSource_WhenGettingCoarseData_ChecksContentAndLengthOfArray()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Day);
            SetupGet(signal);
            SetupMVPGet(new NoneQualityMissingValuePolicyInteger());
            SetupGetData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2016, 9, 13), Value = 7 },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2016, 9, 15), Value = 7 }
            });

            var result = signalsWebService.GetCoarseData(signalId,Dto.Granularity.Week ,new DateTime(2016, 9, 12), new DateTime(2016, 9, 19)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 9, 12), Value = 2},
               };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }

        [TestMethod]
        public void GivenASignal_GetCoarseDataWithSpecificValueMvp_ReturnsIt()
        {
            int signalId = 5;
            Signal signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new SpecificValueMissingValuePolicyDouble() { Quality = Quality.Good, Value = 5.0 });

            var datum = new[] {
               new Domain.Datum<double> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,1), Value = (double)1 }
            };

            var result = signalsWebService.GetCoarseData(signalId,Dto.Granularity.Year ,new DateTime(2017, 1, 1), new DateTime(2019, 1, 1)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2017, 1, 1), Value = 5.0 },
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2018, 1, 1), Value = 5.0 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));

        }

        [TestMethod]
        public void GivenASignal_GetCoarseDataWithNoneQualityMvp_ReturnsNoneData()
        {
            int signalId = 5;
            Signal signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new NoneQualityMissingValuePolicyInteger());

            var datum = new[] {
               new Domain.Datum<int> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,1), Value = (int)1 }
            };

            var result = signalsWebService.GetCoarseData(signalId,Dto.Granularity.Year, new DateTime(2017, 1, 1), new DateTime(2019, 1, 1)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2017, 1, 1), Value = 0 },
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2018, 1, 1), Value = 0 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));

        }

        [TestMethod]
        public void GetCoarseData_SignalGranualityWeek_FirstOrderMissingValuePolicyShouldFillMissingData()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Day);
            SetupGet(signal);
            SetupMVPGet(new FirstOrderMissingValuePolicyInteger());

            var datum = new[] {
               new Domain.Datum<int> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,8,1), Value = (int)1 },
                new Domain.Datum<int> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,8,8), Value = (int)8 },
               new Domain.Datum<int> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2016,8,15), Value = (int)8}
            };

            SetupGetData(datum);

            signalsDataRepositoryMock.Setup(sdr => sdr.GetDataOlderThan<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new[] { datum[0] });


            signalsDataRepositoryMock
                    .Setup(q => q.GetDataNewerThan<int>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns<Signal, DateTime, int>((sig, dateTime, count) =>
                    {
                        return new[] { datum.FirstOrDefault(x => x.Timestamp >= dateTime) };
                    });

            var result = signalsWebService.GetCoarseData(signalId,Dto.Granularity.Week, new DateTime(2016, 8, 1), new DateTime(2016, 8, 15)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1), Value = 4 },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 8, 8), Value = 8 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));

        }
    }
}
