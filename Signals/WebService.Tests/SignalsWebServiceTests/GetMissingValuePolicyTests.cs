using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;
using DataAccess.GenericInstantiations;
using Moq;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class GetMissingValuePolicyTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
        public void GivenNoSignals_GetMissingValuePolicy_ExpectedException()
        {
            SetupGet();

            signalsWebService.GetMissingValuePolicy(1);
        }
        
        [TestMethod]
        public void GivenASignalAddedConfiguration_GetMissingValuePolicy_ReturnsIt()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Double);
            double value = 12.34;
            SetupGet(signal);
            SetupMVPGet(new SpecificValueMissingValuePolicyDouble() {
                Quality = Domain.Quality.Fair,
                Signal = signal,
                Value = value
            });
            
            var result = (Dto.MissingValuePolicy.SpecificValueMissingValuePolicy)
                signalsWebService.GetMissingValuePolicy(signalId);

            Assert.AreEqual(value, result.Value);
            Assert.AreEqual(Dto.Quality.Fair, result.Quality);
        }
    }
}
