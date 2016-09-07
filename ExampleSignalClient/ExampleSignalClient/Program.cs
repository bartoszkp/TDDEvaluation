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
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "ZeroOrderTests4" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy() { DataType = DataType.String });

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 15), Value = "middle" }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 14), new DateTime(2000, 1, 16));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
