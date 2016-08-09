using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var sig = new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "defaultPolicy" } }
            };

            //client.Add(sig);

            client.SetMissingValuePolicy(1, new NoneQualityMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(1, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = true },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = true },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 12, 1), Value = true }
            });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2001, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
