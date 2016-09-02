using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;
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
    public class ZeroQualityDataFillTests
    {
        SignalsWebService signalsWebService;

        [TestMethod]
        public void ZeroQuality_WhenGetOlderThan_ReturnFillData_Double_Month()
        {
            SetupWebService();

            double startvalue = 1;
            double middlevalue = 5;
            double endvalue = 10;

            var collection = GenerateFillCollection<double>(startvalue, middlevalue, endvalue, Granularity.Month);

            signalsDataRepoMock.Setup(x => x.GetDataOlderThan<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp).ToList();


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGetOlderThan_ReturnFillData_Double_Day()
        {
            SetupWebService();

            double startvalue = 1;
            double middlevalue = 5;
            double endvalue = 10;

            var collection = GenerateFillCollection<double>(startvalue, middlevalue, endvalue, Granularity.Day);

            signalsDataRepoMock.Setup(x => x.GetDataOlderThan<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Day,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });

            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);

            var returnCollection = items.ToList();

            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);

        }

        [TestMethod]
        public void ZeroQuality_WhenGetOlderThan_ReturnFillData_Integer_Second()
        {
            SetupWebService();

            int startvalue = 1;
            int middlevalue = 5;
            int endvalue = 10;

            var collection = GenerateFillCollection<int>(startvalue, middlevalue, endvalue, Granularity.Second);

            signalsDataRepoMock.Setup(x => x.GetDataOlderThan<int>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Second,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGetOlderThan_ReturnFillData_Integer_Minute()
        {
            SetupWebService();

            int startvalue = 1;
            int middlevalue = 5;
            int endvalue = 10;

            var collection = GenerateFillCollection<int>(startvalue, middlevalue, endvalue, Granularity.Minute);

            signalsDataRepoMock.Setup(x => x.GetDataOlderThan<int>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Minute,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });

            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);

            var returnCollection = items.ToList();

            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGetOlderThan_ReturnFillData_Decimal_Year()
        {
            SetupWebService();

            decimal startvalue = 1;
            decimal middlevalue = 5;
            decimal endvalue = 10;

            var collection = GenerateFillCollection<decimal>(startvalue, middlevalue, endvalue, Granularity.Year);

            signalsDataRepoMock.Setup(x => x.GetDataOlderThan<decimal>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Year,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGetOlderThan_ReturnFillData_Decimal_Week()
        {
            SetupWebService();

            decimal startvalue = 1;
            decimal middlevalue = 5;
            decimal endvalue = 10;

            var collection = GenerateFillCollection<decimal>(startvalue, middlevalue, endvalue, Granularity.Week);

            signalsDataRepoMock.Setup(x => x.GetDataOlderThan<decimal>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Week,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[2].Value, returnCollection[3].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Double_Month()
        {
            SetupWebService();

            double startvalue = 1;
            double middlevalue = 5;
            double endvalue = 10;

            var collection = GenerateFillCollection<double>(startvalue, middlevalue, endvalue, Granularity.Month);

            signalsDataRepoMock.Setup(x => x.GetData<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Double_Day()
        {
            SetupWebService();

            double startvalue = 1;
            double middlevalue = 5;
            double endvalue = 10;

            var collection = GenerateFillCollection<double>(startvalue, middlevalue, endvalue, Granularity.Day);

            signalsDataRepoMock.Setup(x => x.GetData<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Day,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });

            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);

            var returnCollection = items.ToList();

            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);

        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Integer_Second()
        {
            SetupWebService();

            int startvalue = 1;
            int middlevalue = 5;
            int endvalue = 10;

            var collection = GenerateFillCollection<int>(startvalue, middlevalue, endvalue, Granularity.Second);

            signalsDataRepoMock.Setup(x => x.GetData<int>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Second,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Integer_Minute()
        {
            SetupWebService();

            int startvalue = 1;
            int middlevalue = 5;
            int endvalue = 10;

            var collection = GenerateFillCollection<int>(startvalue, middlevalue, endvalue, Granularity.Minute);

            signalsDataRepoMock.Setup(x => x.GetData<int>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Minute,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });

            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);

            var returnCollection = items.ToList();

            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Decimal_Year()
        {
            SetupWebService();

            decimal startvalue = 1;
            decimal middlevalue = 5;
            decimal endvalue = 10;

            var collection = GenerateFillCollection<decimal>(startvalue, middlevalue, endvalue, Granularity.Year);

            signalsDataRepoMock.Setup(x => x.GetData<decimal>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Year,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[3].Value, returnCollection[4].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Decimal_Week()
        {
            SetupWebService();

            decimal startvalue = 1;
            decimal middlevalue = 5;
            decimal endvalue = 10;

            var collection = GenerateFillCollection<decimal>(startvalue, middlevalue, endvalue, Granularity.Week);

            signalsDataRepoMock.Setup(x => x.GetData<decimal>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(collection);

            mvpRepoMock.Setup(x => x.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

            signalsRepoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Week,
                Id = 1,
                Path = Domain.Path.FromString("x/y")
            });


            var items = signalsWebService.GetData(1, collection.First().Timestamp, collection.Last().Timestamp);


            var returnCollection = items.ToList();


            Assert.AreEqual(returnCollection.First().Value, startvalue);
            Assert.AreEqual(returnCollection.First().Value, returnCollection[1].Value);
            Assert.AreEqual(returnCollection[2].Value, returnCollection[3].Value);
            Assert.AreEqual(returnCollection.Last().Value, endvalue);
        }

        private List<Datum<T>> GenerateFillCollection<T>(T startvalue, T middlevalue, T endvalue, Domain.Granularity granulatiry)
        {
            List<Datum<T>> ListOfData = new List<Datum<T>>();

        switch(granulatiry)
            {
                case Granularity.Day:
                    {
                        for(int i = 0; i < 3; i++)
                        {
                            var newItem = new Datum<T>()
                            {
                                Id = 1,
                                Quality = Quality.Good,
                                Timestamp = new DateTime(2000, 1, 1 + (i * 3))
                            };
                            if (i == 0)
                                newItem.Value = startvalue;
                            else if (i == 1)
                                newItem.Value = middlevalue;
                            else
                                newItem.Value = endvalue;

                            ListOfData.Add(newItem);
                        }

                        break;
                    }
                case Granularity.Hour:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var newItem = new Datum<T>()
                            {
                                Id = 1,
                                Quality = Quality.Good,
                                Timestamp = new DateTime(2000, 1, 1, 1 + (i * 3), 0, 0)
                            };
                            if (i == 0)
                                newItem.Value = startvalue;
                            else if (i == 1)
                                newItem.Value = middlevalue;
                            else
                                newItem.Value = endvalue;

                            ListOfData.Add(newItem);
                        }

                        break;
                    }
                case Granularity.Minute:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var newItem = new Datum<T>()
                            {
                                Id = 1,
                                Quality = Quality.Good,
                                Timestamp = new DateTime(2000, 1, 1, 1, 1 + (i * 3), 0)
                            };
                            if (i == 0)
                                newItem.Value = startvalue;
                            else if (i == 1)
                                newItem.Value = middlevalue;
                            else
                                newItem.Value = endvalue;

                            ListOfData.Add(newItem);
                        }
                        break;
                    }
                case Granularity.Month:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var newItem = new Datum<T>()
                            {
                                Id = 1,
                                Quality = Quality.Good,
                                Timestamp = new DateTime(2000,1 + (i * 3), 1, 0, 0, 0)
                            };
                            if (i == 0)
                                newItem.Value = startvalue;
                            else if (i == 1)
                                newItem.Value = middlevalue;
                            else
                                newItem.Value = endvalue;

                            ListOfData.Add(newItem);
                        }

                        break;
                    }
                case Granularity.Second:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var newItem = new Datum<T>()
                            {
                                Id = 1,
                                Quality = Quality.Good,
                                Timestamp = new DateTime(2000, 1, 1, 1, 1, 1 + (i * 3))
                            };
                            if (i == 0)
                                newItem.Value = startvalue;
                            else if (i == 1)
                                newItem.Value = middlevalue;
                            else
                                newItem.Value = endvalue;

                            ListOfData.Add(newItem);
                        }


                        break;
                    }
                case Granularity.Week:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var time = new DateTime(2000, 1, 3);
                            var newItem = new Datum<T>()
                            {
                                Id = 1,
                                Quality = Quality.Good,
                                Timestamp = time.AddDays(14 * i)
                            };
                            if (i == 0)
                                newItem.Value = startvalue;
                            else if (i == 1)
                                newItem.Value = middlevalue;
                            else
                                newItem.Value = endvalue;

                            ListOfData.Add(newItem);
                        }

                        break;
                    }
                case Granularity.Year:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var newItem = new Datum<T>()
                            {
                                Id = 1,
                                Quality = Quality.Good,
                                Timestamp = new DateTime(2000 + (i* 3), 1, 1, 0, 0, 0)
                            };
                            if (i == 0)
                                newItem.Value = startvalue;
                            else if (i == 1)
                                newItem.Value = middlevalue;
                            else
                                newItem.Value = endvalue;

                            ListOfData.Add(newItem);
                        }

                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            


            return ListOfData;
        }


        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }


        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

    }
}
