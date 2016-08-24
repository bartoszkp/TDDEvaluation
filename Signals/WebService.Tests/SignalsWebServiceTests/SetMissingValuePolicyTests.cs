using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;
using DataAccess.GenericInstantiations;
using Dto.MissingValuePolicy;
using Moq;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class SetMissingValuePolicyTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
        public void GivenNoSignals_SetMissingValuePolicy_ExpectedException()
        {
            SetupGet();
            
            signalsWebService.SetMissingValuePolicy(1, new SpecificValueMissingValuePolicy()
            {
                DataType = Dto.DataType.Double,
                Quality = Dto.Quality.Good,
                Value = 2.5
            });
        }
        
        [TestMethod]
        public void GivenASignal_SetMissingValuePolicy_CheckIsSetted()
        {
            var signalId = 1;
            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Double));

            signalsWebService.SetMissingValuePolicy(signalId, new SpecificValueMissingValuePolicy(){
                DataType = Dto.DataType.Double,
                Quality = Dto.Quality.Good,
                Value = 1.2
            });

            missingValuePolicyRepoMock
               .Verify(mvp => mvp.Set(It.Is<Domain.Signal>(sig => sig.Id == signalId),
                   It.IsAny<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>()));
        } 
    }
}
