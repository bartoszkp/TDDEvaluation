using System;
using System.Linq;
using System.Collections.Generic;
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
            public void GivenNoSignals_WhenGettingByAnyPath_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>())).Returns<Signal>(null);

                var result = signalsWebService.Get ( new Dto.Path() { Components=new string[] { "root", "signal" } } );

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByAnyId_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns<Signal>(null);

                var result = signalsWebService.GetById(1);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByOtherPath_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Path>(path => path.Components.ToArray() == new string[] { "root", "signal1"} )))
                    .Returns(new Signal());
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Path>(path => path.Components.ToArray() == new string[] { "root", "signal2" })))
                    .Returns<Signal>(null);

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal2" } });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByOtherId_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                signalsRepositoryMock.Setup(sr => sr.Get(2)).Returns<Signal>(null);

                var result = signalsWebService.GetById(2);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByTheSignalsPath_ReturnedIsNotNullResult()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Path>(path => path.Equals(Path.FromString("root/signal1")))))
                    .Returns(new Signal());

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal1" } });

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByTheSignalsId_ReturnedIsNotNullResult()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());

                var result = signalsWebService.GetById(1);

                Assert.IsNotNull(result);
            }



            // --------------------------------------------------------------------------------------------------------------------------


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingByAnyPath_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                var dummyPath = Path.FromString("root/signal");

                signalWebService.Get(dummyPath.ToDto<Dto.Path>());
                
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingByPathWhichDoesNotExistInRepository_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                signalWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Decimal,
                    Granularity = Dto.Granularity.Hour,
                    Path = new Dto.Path() { Components = new string[] { "root", "signal1" } }
                });
                var dummyPath = Path.FromString("root/signal2");

                signalWebService.Get(dummyPath.ToDto<Dto.Path>());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPathWhichExistsInRepository_ReturnedIsTheSignal()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                var dummySignal = new Dto.Signal()
                {
                    DataType = Dto.DataType.Decimal,
                    Granularity = Dto.Granularity.Hour,
                    Path = new Dto.Path() { Components = new string[] { "root", "signal1" } }
                };
                var dummyPath = Path.FromString("root/signal1");
                signalRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(path => path.Equals(dummyPath)))).Returns(dummySignal.ToDomain<Domain.Signal>());

                var result = signalWebService.Get(dummyPath.ToDto<Dto.Path>());

                Assert.AreEqual(dummySignal.Granularity, result.Granularity);
                Assert.AreEqual(dummySignal.DataType, result.DataType);
                CollectionAssert.AreEqual(dummySignal.Path.Components.ToArray(), result.Path.Components.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingMissingValuePolicyOfAnySignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns<Signal>(signal => signal);
                signalRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>())).Returns<Signal>(signal => signal);
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingMissingValuePolicyOfSignalWhichDoesNotExistInRepository_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns<Signal>(signal => signal);
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.GetMissingValuePolicy(1);
            }

            

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingDataOfAnySignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.GetData(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingDataOfOtherSignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                signalWebService.Add(new Dto.Signal() { Id = 1 });

                signalWebService.GetData(2, It.IsAny<DateTime>(), It.IsAny<DateTime>());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataOfTheSignal_ReturnedIsTheData()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1 });
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalDataRepositoryMock
                    .Setup(sdr => sdr.GetData<double>(It.Is<Signal>(signal => signal.Id == 1), new DateTime(2015, 1, 1), new DateTime(2017, 1, 1)))
                    .Returns(new Datum<double>[] { new Datum<double>() { } });
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                signalWebService.Add(new Dto.Signal() { Id = 1 });

                var expected = new Datum<double>[] { new Datum<double>() { } }.ToDto<IEnumerable<Dto.Datum>>().ToArray();
                var actual = signalWebService.GetData(1, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1)).ToArray();

                Assert.AreEqual(expected[0].Quality, actual[0].Quality);
                Assert.AreEqual(expected[0].Timestamp, actual[0].Timestamp);
                Assert.AreEqual(expected[0].Value, actual[0].Value);
            }

            [TestMethod]
            public void GivenASignalWithoutSetMVP_WhenGettingMVPofTheSignal_ReturnedIsNull()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalMissingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, signalMissingValuePolicyRepositoryMock.Object);
                var signalWebService = new SignalsWebService(signalDomainService);
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1 });
                signalMissingValuePolicyRepositoryMock
                    .Setup(smvpr => smvpr.Get(It.Is<Signal>(signal => signal.Id == 1)))
                    .Returns<Domain.MissingValuePolicy.MissingValuePolicyBase>(null);

                var result = signalWebService.GetMissingValuePolicy(1);

                Assert.IsNull(result);

            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataOfTheSignalAndGettingTheData_GotIsNotNullData()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalDataRepositoryMock.Setup(sdr => sdr.GetData<double>(It.Is<Signal>(signal => signal.Id == 1), new DateTime(2015, 1, 1), new DateTime(2017, 1, 1)))
                    .Returns(new Datum<double>[] { new Datum<double>() { Timestamp = new DateTime(2016, 1, 1), Value = 10.0, Quality = Quality.Fair } });
                signalWebService.SetData(1, new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 10.0 } });
                var result = signalWebService.GetData(1, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1));

                Assert.IsNotNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenSettingDataOfAnySignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.SetData(1, null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingDataOfOtherSignal_ReturnedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.SetData(2, null);
            }

            [TestMethod]
            public void GivenASignalWithSetMVP_WhenGettingMVPofTheSignal_ReturnedIsNotNullResult()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalMissingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, signalMissingValuePolicyRepositoryMock.Object);
                var signalWebService = new SignalsWebService(signalDomainService);
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1 });
                signalMissingValuePolicyRepositoryMock
                    .Setup(smvpr => smvpr.Get(It.Is<Signal>(signal => signal.Id == 1)))
                    .Returns(new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>());

                var result = signalWebService.GetMissingValuePolicy(1);

                Assert.IsNotNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenSettingMVPofAnySignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalMissingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, signalMissingValuePolicyRepositoryMock.Object);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.SetMissingValuePolicy(1, null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingMVPofOtherSignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                var signalMissingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, signalMissingValuePolicyRepositoryMock.Object);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.SetMissingValuePolicy(2, null);


            }

            [TestMethod]
            public void GivenASignal_WhenSettingMVPofTheSignalAndGettingMVPofTheSignal_ReturnedIsMVPofTheSignal()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                var signalMissingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, signalMissingValuePolicyRepositoryMock.Object);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.SetMissingValuePolicy(1
                    , new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { Id = 1, DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Fair });
                var result = signalWebService.GetMissingValuePolicy(1);

                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(Dto.DataType.Decimal, result.DataType);
            }

            


        }
    }
}