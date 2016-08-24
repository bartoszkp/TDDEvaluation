using System;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class SignalsWebServiceAddTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
        public void GivenNoSignals_WhenAddingASignalWithId_ThrowIdNotNullException()
        {
            var signalId = 1;
            SetupAdd(signalId);

            signalsWebService.Add(Utils.SignalWith(id: signalId, dataType: Dto.DataType.Boolean));
        }
        
        [TestMethod]
        public void GivenNoSignals_WhenAddingASignal_ReturnsIt()
        {
            var signalId = 1;
            SetupAdd(signalId);

            var signal = Utils.SignalWith(path: new Dto.Path() { Components = new[] { "a" } });
            var result = signalsWebService.Add(signal);

            Assert.AreEqual(signal.DataType, result.DataType);
            Assert.AreEqual(signal.Granularity, result.Granularity);
            CollectionAssert.AreEqual(signal.Path.Components.ToArray(), result.Path.Components.ToArray());
        }

        [TestMethod]
        public void GivenNoSignals_WhenAddingASignal_CallsRepositoryAdd()
        {
            var signalId = 1;
            SetupAdd(signalId);

            signalsWebService.Add(Utils.SignalWith(
                null, 
                Dto.DataType.Double, 
                Dto.Granularity.Day, 
                new Dto.Path() { Components = new[] { "a" } }
            ));

            signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                => passedSignal.DataType == Domain.DataType.Double
                    && passedSignal.Granularity == Domain.Granularity.Day
                    && passedSignal.Path.ToString() == "a")));
        }

        [TestMethod]
        public void GivenNoSignals_WhenAddingASignal_DefaultMissingPolicyIsSet()
        {
            SetupAdd(1);

            var result = signalsWebService.Add(Utils.SignalWith(null, Dto.DataType.Double));

            missingValuePolicyRepoMock
                .Verify(mvp => mvp.Set(It.IsAny<Domain.Signal>(),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
        }
    }
}
