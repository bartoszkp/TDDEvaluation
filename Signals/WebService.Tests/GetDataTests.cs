using Domain.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Tests
{
    [TestClass]
    public class GetDataTests
    {

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void IncorrectInterval_GetData_ReturnsEmptyCollection()
        {
            SignalsDomainService domainService = new SignalsDomainService(null, null, null);
            signalsWebService = new SignalsWebService(domainService);

            var result =  signalsWebService.GetData(1, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(0, result.Count());

        }
        

    }
}
