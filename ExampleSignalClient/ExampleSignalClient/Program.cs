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
                DataType = DataType.Boolean,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "ZeroOrderTesasdtasass" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy() { DataType = DataType.Boolean });

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = false },
    new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 5), Value = true }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 1, 10));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
