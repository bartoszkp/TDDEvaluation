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
                Path = new Path() { Components = new[] { "FirstOrderTests4" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Integer });

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2018, 2, 1), Value = 10 },
                new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2018, 4, 1), Value = 30 }
            });

            var result = client.GetData(id, new DateTime(2018, 1, 1), new DateTime(2018, 6, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
