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
                DataType = DataType.String,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "FirstOrderT3" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.String });

            client.SetData(id, new Datum[]
            {
            new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = "a" },
            new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = "b" },
            new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = "c" }
            });

            var result = client.GetData(id, new DateTime(2000, 6, 1), new DateTime(2000, 7, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
