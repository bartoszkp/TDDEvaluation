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
                DataType = DataType.Integer,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "FirstOrderTe18" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Integer });

            client.SetData(id, new Datum[]
            {
        new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
        new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 5), Value = 2 },
        new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 8), Value = 5 }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 6), new DateTime(2000, 1, 7));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
