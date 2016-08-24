using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using DataAccess.GenericInstantiations;
using System.Collections.Generic;

namespace WebService.Tests
{
    [TestClass]
    public class SignalsWebServiceGetDataTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
        public void GivenNoSignals_GetDataByIdAndTime_ExpectedException()
        {
            var signalId = 1;
            Domain.Signal signal = null;
            Setup(signalId, signal);

            signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
        }
        
        [TestMethod]
        public void GivenASignalWithNoDataSource_WhenGettingData_ChecksLengthOfArray()
        {
            var signalId = 1;
            Setup(signalId, Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Day));
            SetupMVP(new NoneQualityMissingValuePolicyDouble());

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 1, 21));
            
            Assert.AreEqual(20, result.Count());
        }

        [TestMethod]
        public void GivenASignalWithDataSource_WhenGettingData_ChecksContentAndLengthOfArray()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new NoneQualityMissingValuePolicyInteger());
            SetupData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 5 },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 4, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 5},
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2005, 2, 1), Value = default(int)},
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithCompleteDataSource_WhenGettingSortedgData_ChecksContentAndLengthOfArray()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new NoneQualityMissingValuePolicyInteger());
            SetupData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7, },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 2, 1), Value = 2 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 4, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 2, 1), Value = 2 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithIncompleteDataSource_WhenGettingSortedData_ChecksContentAndLengthOfArray()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new NoneQualityMissingValuePolicyInteger());
            SetupData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7, },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 11 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 4, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2005, 2, 1), Value = default(int) },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithData_WhenGettingDataWithFromIncludedUtcEqualsToExcludedUtc_ReturnsExpectedValue()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new NoneQualityMissingValuePolicyInteger());
            SetupData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = (int)11 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 1, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithSpecificMissingValuePolicy_WhenGettingData_ReturnsDataWithFilledWithSpecificValue()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new SpecificValueMissingValuePolicyInteger() { Quality = Domain.Quality.Poor, Value = 16 });
            SetupData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7, },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 11 }
            });
            
            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 6, 1)).ToArray();
            var expected =new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 2, 1), Value = 16 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 4, 1), Value = 16 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 5, 1), Value = 16 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithOutData_WhenGettingDataFromSpecificPointInTime_ReturnsDataWithSpecificValue()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new SpecificValueMissingValuePolicyInteger() { Quality = Domain.Quality.Poor, Value = 16 });
            SetupData(new List<Domain.Datum<Int32>>());

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 1, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 1, 1), Value = 16 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignal_WhenGettingDataWithZeroOrderMissingValuePolicy_ReturnsIt()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new ZeroOrderMissingValuePolicyDouble());
            SetupData(new[] {
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.1 },
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 3.3 },
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 5.5 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1)).ToArray();
            var expected = new[]{
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.1 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 1.1 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 3.3 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 3.3 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 5.5 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignal_WhenGettingDataWithZeroOrderMissingValuePolicy_ReturnsDefault()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month);
            Setup(signalId, signal);
            SetupMVP(new ZeroOrderMissingValuePolicyDouble());
            SetupData(new[] {
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 2.0 },
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 4.0 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(double) },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 2.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 4.0 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }

        protected override void Setup(params object[] param)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(It.Is<int>(id => id == (int)param[0])))
                .Returns(param[1] as Domain.Signal);
        }

        private void SetupData<T>(IEnumerable<Domain.Datum<T>> data)
        {
            signalsDataRepositoryMock
                .Setup(sdr => sdr.GetData<T>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(data);
        }

        private void SetupMVP(Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            missingValuePolicyRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(missingValuePolicy);
        }
    }
}
