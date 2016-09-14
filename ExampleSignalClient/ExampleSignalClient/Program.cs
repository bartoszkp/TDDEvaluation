using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {

        static void Main(string[] args)
        {
            var rng = new Random();
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "FirstOrderBugTests"  + rng.Next().ToString() } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 7, 1), Value = (double)2.5 }
            });

                var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }
            Console.WriteLine("Done");

            Console.ReadKey();

        }
    }
}
