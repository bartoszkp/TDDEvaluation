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
            SetupAdd();

            signalsWebService.Add(Utils.SignalWith(id: 1, dataType: Dto.DataType.Boolean));
        }
        
        [TestMethod]
        public void GivenNoSignals_WhenAddingASignal_ReturnsIt()
        {
            SetupAdd();

            var signal = Utils.SignalWith(path: new Dto.Path() { Components = new[] { "a" } });
            var result = signalsWebService.Add(signal);

            Assert.AreEqual(signal.DataType, result.DataType);
            Assert.AreEqual(signal.Granularity, result.Granularity);
            CollectionAssert.AreEqual(signal.Path.Components.ToArray(), result.Path.Components.ToArray());
        }

        [TestMethod]
        public void GivenNoSignals_WhenAddingASignal_CallsRepositoryAdd()
        {
            SetupAdd();

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
    }
}
