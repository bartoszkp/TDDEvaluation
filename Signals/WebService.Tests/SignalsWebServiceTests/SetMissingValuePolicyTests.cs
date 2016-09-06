using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;
using DataAccess.GenericInstantiations;
using Dto.MissingValuePolicy;
using Moq;
using System.Reflection;

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

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void GivenASignal_ShadowMissingValuePolicy_WhenSignalsAreDifferent_ExpectedException()
        {
            int signalId = 5;
            Dto.Signal shadowSignal = new Dto.Signal()
            {
                DataType = Dto.DataType.Double,
                Granularity = Dto.Granularity.Minute,
                Path = new Dto.Path { Components = new[] { "aaa" } }
            };

            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Hour));

            signalsWebService.SetMissingValuePolicy(
                signalId,
                new ShadowMissingValuePolicy {DataType = Dto.DataType.Double, ShadowSignal = shadowSignal });
        }

        [TestMethod]
        public void GivenASignal_ShadowMissingValuePolicy_WhenSignalsAreDifferent_CheckIsSetted()
        {
            int signalId = 5;
            Dto.Signal shadowSignal = new Dto.Signal()
            {
                DataType = Dto.DataType.Double,
                Granularity = Dto.Granularity.Minute,
                Path = new Dto.Path { Components = new[] { "aaa" } }
            };

            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Minute));

            signalsWebService.SetMissingValuePolicy(
               signalId,
               new ShadowMissingValuePolicy { DataType = Dto.DataType.Double, ShadowSignal = shadowSignal });

            missingValuePolicyRepoMock
             .Verify(mvp => mvp.Set(It.Is<Domain.Signal>(sig => sig.Id == signalId),
                 It.IsAny<Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>>()));
        }
    }
}
