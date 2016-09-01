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
                Granularity = Granularity.Year,
                Path = new Path() { Components = new[] { "FirstOrderTe14" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Integer });

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2005, 1, 1), Value = 2 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2008, 1, 1), Value = 5 }
            });

            var result = client.GetData(id, new DateTime(1997, 1, 1), new DateTime(2011, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
