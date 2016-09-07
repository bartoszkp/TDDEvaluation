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
                Path = new Path() { Components = new[] { "ZeroOrderTests2" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy() { DataType = DataType.Decimal });

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2018, 1, 1), Value = 1m }
            });

            var result = client.GetData(id, new DateTime(2018, 1, 8), new DateTime(2018, 1, 15));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
