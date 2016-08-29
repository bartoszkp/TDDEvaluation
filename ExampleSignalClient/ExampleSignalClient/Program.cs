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
                Granularity = Granularity.Year,
                Path = new Path() { Components = new[] { "year1" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy() { DataType = DataType.Boolean });
            client.SetData(id, new Datum[]
            {
    new Datum() { Timestamp = new DateTime(2000, 1, 1), Value = true, Quality = Quality.Good },
    new Datum() { Timestamp = new DateTime(2003, 1, 1), Value = true, Quality = Quality.Good }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2004, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}

