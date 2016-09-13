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
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "ZeroOrderTests" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy() { DataType = DataType.Decimal });

            client.SetData(id, new Datum[]
            {
                 new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2018, 1, 1), Value = 1m },
                 new Datum() { Quality = Quality.None, Timestamp = new DateTime(2018, 1, 5), Value = 0m }
            });

            var result = client.GetData(id, new DateTime(2018, 1, 1), new DateTime(2018, 1, 10));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();


        }
    }
}
