using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {

            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Second,
                Path = new Path() { Components = new[] { "FirstOrderTe11161" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(id, new Datum[]
            {
        new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1), Value = 1.0 },
        new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 5), Value = 2.0 },
        new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 1, 8), Value = 5.0 }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1, 1, 0, 59), new DateTime(2000, 1, 1, 1, 1, 10));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
