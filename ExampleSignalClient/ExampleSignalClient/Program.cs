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
                DataType = DataType.Decimal,
                Granularity = Granularity.Week,
                Path = new Path() { Components = new[] { "FirstOrderTests" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Decimal });

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 8, 29), Value = 1m },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 9, 19), Value = 2m },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 10, 3), Value = 5m }
            });

            var result = client.GetData(id, new DateTime(2016, 8, 22), new DateTime(2016, 10, 10));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
