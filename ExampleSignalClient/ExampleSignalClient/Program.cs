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
                Granularity = Granularity.Hour,
                Path = new Path() { Components = new[] { "FirstOrderTe32" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(id, new Datum[]
            {
        new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = 1.0 },
        new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = 2.0 },
        new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 8, 0, 0), Value = 5.0 }
            });

            var result = client.GetData(id, new DateTime(1999, 12, 31, 22, 0, 0), new DateTime(2000, 1, 1, 10, 0, 0));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
