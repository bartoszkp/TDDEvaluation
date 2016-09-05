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
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "FirstOrderTestsDoub" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(id, new Datum[]
            {
                new Datum() { Timestamp = new DateTime(2000, 1, 2), Value = 10.2d, Quality = Quality.Good },
                new Datum() { Timestamp = new DateTime(2000, 1, 6), Value = 30.5d, Quality = Quality.Good }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
