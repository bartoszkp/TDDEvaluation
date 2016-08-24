using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class SignalsWebServiceSetDataTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
        public void GivenNoSignals_WhenSetData_ExpectedException()
        {
            signalsWebService.SetData(1, new Dto.Datum[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = true },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = false },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = true }
            });
        }
        
        [TestMethod]
        public void GivenASignal_WhenSettingSignalDataWithInvalidMilliseconds_ExpectedException()
        {
            Assert.IsTrue(IsSetDataTimestampValid(Utils.validTimestamp.AddMilliseconds(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenSettingSignalDataWithInvalidSeconds_ExpectedException()
        {
            Assert.IsTrue(IsSetDataTimestampValid(Utils.validTimestamp.AddSeconds(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenSettingSignalDataWithInvalidMinutes_ExpectedException()
        {
            Assert.IsTrue(IsSetDataTimestampValid(Utils.validTimestamp.AddMinutes(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenSettingSignalDataWithInvalidHours_ExpectedException()
        {
            Assert.IsTrue(IsSetDataTimestampValid(Utils.validTimestamp.AddHours(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenSettingSignalDataWithInvalidWeekDay_ExpectedException()
        {
            Assert.IsTrue(IsSetDataTimestampValid(Utils.validTimestamp, Domain.Granularity.Week));
        }

        [TestMethod]
        public void GivenASignal_WhenSettingSignalDataWithInvalidFirstDayOfMonth_ExpectedException()
        {
            Assert.IsTrue(IsSetDataTimestampValid(Utils.validTimestamp.AddDays(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenSettingSignalDataWithInvalidMonths_ExpectedException()
        {
            Assert.IsTrue(IsSetDataTimestampValid(Utils.validTimestamp.AddMonths(1)));
        }
        
        private bool IsSetDataTimestampValid(DateTime dt, Domain.Granularity granularity = Domain.Granularity.Year)
        {
            var signalId = 1;
            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Integer, granularity));

            try
            {
                signalsWebService.SetData(signalId, new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = dt, Value = 0 }
                });
                return false;
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                return (ex.InnerException.GetType() == typeof(Domain.Exceptions.DatetimeIsInvalidException));
            }
        }
    }
}
