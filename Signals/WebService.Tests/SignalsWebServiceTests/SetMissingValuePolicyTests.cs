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
            int shadowId = 2;
            Dto.Signal shadowSignal = new Dto.Signal()
            {
                DataType = Dto.DataType.Double,
                Granularity = Dto.Granularity.Minute,
                Path = new Dto.Path { Components = new[] { "aaa" } },
                Id=shadowId
                
            };

            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Minute));

            signalsWebService.SetMissingValuePolicy(
               signalId,
               new ShadowMissingValuePolicy { DataType = Dto.DataType.Double, ShadowSignal = shadowSignal,Id=shadowId });

            missingValuePolicyRepoMock
             .Verify(mvp => mvp.Set(It.Is<Domain.Signal>(sig => sig.Id == signalId),
                 It.IsAny<Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>>()));
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void GivenASignal_ShadowMissingValuePolicy_WhenSignalsMakeCycle_CheckIsSetted()
        {
            int signalId1 = 1;
            int signalId2 = 2;
            int signalId3 = 3;
            Dto.Signal signal1 = new Dto.Signal()
            {
                DataType = Dto.DataType.Double,
                Granularity = Dto.Granularity.Minute,
                Path = new Dto.Path { Components = new[] { "aaa" } },
                Id = signalId1
            };
            Domain.Signal signal2 = new Domain.Signal()
            {
                DataType =Domain.DataType.Double,
                Granularity = Domain.Granularity.Minute,
     
                Id = signalId2

            };
            Domain.Signal signal3 = new Domain.Signal()
            {
                DataType = Domain.DataType.Double,
                Granularity = Domain.Granularity.Minute,
                Id = signalId3

            };

            SetupGet(Utils.SignalWith(signalId1, Domain.DataType.Double, Domain.Granularity.Minute));
            SetupGet(Utils.SignalWith(signalId2, Domain.DataType.Double, Domain.Granularity.Minute));
            SetupGet(Utils.SignalWith(signalId3, Domain.DataType.Double, Domain.Granularity.Minute));

            Domain.MissingValuePolicy.ShadowMissingValuePolicy<double> missingValuePolicy1 = new Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>() { Id = signalId1, ShadowSignal = signal2 };

            missingValuePolicyRepoMock
              .Setup(mvp => mvp.Get(It.Is<Domain.Signal>(x=>x.Id==signalId1)))
              .Returns(missingValuePolicy1);

            Domain.MissingValuePolicy.ShadowMissingValuePolicy<double> missingValuePolicy2 = new Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>() { Id = signalId2, ShadowSignal = signal3 };

            missingValuePolicyRepoMock
              .Setup(mvp => mvp.Get(It.Is<Domain.Signal>(x => x.Id == signalId2)))
              .Returns(missingValuePolicy2);

            signalsWebService.SetMissingValuePolicy(
              signalId3,
              new ShadowMissingValuePolicy { DataType = Dto.DataType.Double, ShadowSignal = signal1, Id = signalId3 });

        }
    }
}
