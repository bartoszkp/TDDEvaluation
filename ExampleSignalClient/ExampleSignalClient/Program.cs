using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "example", "path6" } },
            });

            client.SetMissingValuePolicy(signal.Id.Value, new ZeroOrderMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(signal.Id.Value, new Datum[]
            {
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = (double)1.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (double)2.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 6, 1), Value = (double)3.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = (double)4.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 9, 1), Value = (double)5.5 }
            });

            var result = client.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 10, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
