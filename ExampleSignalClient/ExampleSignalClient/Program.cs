using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var newSignal = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "root", "signal1" } }
            //};

            //var id = client.Add(newSignal).Id.Value;
            var id = 0;
            client.SetData(id, new Datum[] {
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 15), Value = (double)1.5 },
                new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            client.SetData(id, new Datum[] {
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)4 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 15), Value = (double)22 },
                new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            Console.ReadKey();
        }
    }
}
