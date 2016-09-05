using DataAccess.GenericInstantiations;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Tests
{
    [TestClass]
    public class ShadowMvpTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void SetMvp_WithNullSignalField_ExceptionIsThrown()
        {
            SetupWebService();
            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy();
            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };


            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            signalsWebService.SetMissingValuePolicy(1, mvp);
        }

        [TestMethod]
        [ExpectedException(typeof(ShadowSignalNotMatchingException))]
        public void SetMvp_NotMatchingDataTypesOrGranularities_ExceptionIsThrown()
        {
            SetupWebService();
            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy();
            mvp.ShadowSignal = new Dto.Signal()
            {
                DataType = Dto.DataType.Decimal,
                Granularity = Dto.Granularity.Day
            };

            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };


            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            signalsWebService.SetMissingValuePolicy(1, mvp);

        }







        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();



    }
}
