using System;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class SignalsWebServiceAddTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
        public void GivenNoSignals_WhenAddingASignalWithId_ThrowIdNotNullException()
        {
            Setup();

            signalsWebService.Add(Utils.SignalWith(id: 1));
        }
        
        [TestMethod]
        public void GivenNoSignals_WhenAddingASignal_ReturnsIt()
        {
            Setup();

            var signal = Utils.SignalWith(path: new Dto.Path() { Components = new[] { "a" } });
            var result = signalsWebService.Add(signal);

            Assert.AreEqual(signal.DataType, result.DataType);
            Assert.AreEqual(signal.Granularity, result.Granularity);
            CollectionAssert.AreEqual(signal.Path.Components.ToArray(), result.Path.Components.ToArray());
        }

        [TestMethod]
        public void GivenNoSignals_WhenAddingASignal_CallsRepositoryAdd()
        {
            Setup();

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

        protected override void Setup()
        {
            signalsRepositoryMock
                .Setup(f => f.Add(It.IsAny<Domain.Signal>()))
                .Returns<Domain.Signal>(signal => signal);
        }
    }
}
