using DataAccess.GenericInstantiations;
using Domain;
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
        [ExpectedException(typeof(Exception))]
        public void SetShadowMvp_WhenSignalsShadowIsItself_ExceptionIsThrown() {
            SetupWebService();

            var signal1 = new Signal() {
                Id = 1,
                DataType = DataType.Boolean,
                Granularity = Granularity.Month
            };

            var signal2 = new Signal() {
                Id = 2,
                DataType = DataType.Boolean,
                Granularity = Granularity.Month
            };
            var signal3 = new Signal() {
                Id = 3,
                DataType = DataType.Boolean,
                Granularity = Granularity.Month
            };

            var signal1Mvp = new ShadowMissingValuePolicyBoolean() { Signal = signal1, ShadowSignal = signal2 };
            var signal2Mvp = new ShadowMissingValuePolicyBoolean() { Signal = signal2, ShadowSignal = signal3 };
            var signal3Mvp = new ShadowMissingValuePolicyBoolean() { Signal = signal3, ShadowSignal = signal1 };

            missingValuePolicyRepositoryMock.Setup(s => s.Get(It.Is<Signal>(s2 => s2.Id == 2))).Returns(signal2Mvp);
            missingValuePolicyRepositoryMock.Setup(s => s.Get(It.Is<Signal>(s3 => s3.Id == 3))).Returns(signal3Mvp);
            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(signal1);


            signalsWebService.SetMissingValuePolicy(1,
                new Dto.MissingValuePolicy.ShadowMissingValuePolicy() {
                    DataType = Dto.DataType.Boolean,
                    Signal = new Dto.Signal() {Granularity = Dto.Granularity.Month,DataType = Dto.DataType.Boolean,Id = 1 },
                    ShadowSignal = new Dto.Signal() { Granularity = Dto.Granularity.Month,DataType = Dto.DataType.Boolean, Id = 2 }
                });



        }




        private void SetupWebService() {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepositoryMock.Object,
                signalDataRepositoryMock.Object,
                missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

    }
}
