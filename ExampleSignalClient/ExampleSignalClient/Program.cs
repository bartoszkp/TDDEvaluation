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
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "ZeroOrderTests" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(id, new Datum[]
            {
                 new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1.0 },
                 new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 5.0 }
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
