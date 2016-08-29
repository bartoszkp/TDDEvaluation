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
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "Month1" } }
            }).Id.Value;

            client.SetData(id, new[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 14 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 28 }
            });

            client.SetMissingValuePolicy(id, new SpecificValueMissingValuePolicy()
            {
                DataType = DataType.Integer,
                Quality = Quality.Fair,
                Value = -1
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
