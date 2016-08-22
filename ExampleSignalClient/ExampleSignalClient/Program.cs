using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal1 = new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "signal1" } }
            };
            var signal2 = new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "signal2 " } }
            };

            var signalId1 = client.Add(signal1).Id.Value;
            var signalId2 = client.Add(signal2).Id.Value;

            client.SetData(
                signalId1,
                 new Datum[] { new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = false } });
            client.SetData(
                signalId2,
                new Datum[] { new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = true } });

            var data1 = client.GetData(signalId1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
            var data2 = client.GetData(signalId2, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            foreach (var d in data1)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }
            foreach (var d in data2)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();
        }
    }
}
