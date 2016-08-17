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
                Path = new Path() { Components = new[] { "root", "someSignal" } }
            };

            client.Add(sig);

            client.SetData(1, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = true },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = true },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = true }
            });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
