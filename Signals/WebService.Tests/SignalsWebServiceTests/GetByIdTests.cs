using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class SignalsWebServiceGetByIdTests : SignalsWebServiceRepository
    {
        [TestMethod]
        public void GivenNoSignals_WhenGettingById_ReturnsNull()
        {
            SetupGet();

            var result = signalsWebService.GetById(0);

            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void GivenASignal_WhenGettingByItsId_ReturnsIt()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Boolean, Domain.Granularity.Hour, Domain.Path.FromString("a/b"));
            SetupGet(signal);

            var result = signalsWebService.GetById(signalId);

            Assert.AreEqual(signalId, result.Id);
            Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
            Assert.AreEqual(Dto.Granularity.Hour, result.Granularity);
            CollectionAssert.AreEqual(new[] { "a", "b" }, result.Path.Components.ToArray());
        }
    }
}
