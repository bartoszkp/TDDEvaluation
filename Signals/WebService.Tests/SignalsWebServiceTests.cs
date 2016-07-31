using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_DoesNotThrow()
            {
                var signalsWebService = new SignalsWebService(null);
                signalsWebService.Add(new Dto.Signal());
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            {
                var signalsWebService = new SignalsWebService(null);

                var result = signalsWebService.Add(new Dto.Signal());

                Assert.IsNotNull(result);
            }
        }
    }
}