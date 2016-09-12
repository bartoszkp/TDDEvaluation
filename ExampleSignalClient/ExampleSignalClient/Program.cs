using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal() {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "GetTsTsTests13224" } }
            }).Id.Value;


            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy() { DataType = DataType.Double});

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
            });

            var result = client.GetData(id, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1));

            foreach (var d in result) {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }
            Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}
