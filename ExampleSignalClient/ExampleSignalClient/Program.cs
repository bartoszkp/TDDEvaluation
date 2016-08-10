using System;
using ExampleSignalClient.Signals;
using System.Linq;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var newSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Minute,
                Path = new Path() { Components = new[] { "root", "defaultPolicy354" } }
            };

            int signalId = client.Add(newSignal).Id.Value;

            client.SetData(signalId, new Datum[] {
                         new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            var result = client.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 8, 1));

            Console.WriteLine(result.Count());
            Console.ReadKey();
        }
    }
}
