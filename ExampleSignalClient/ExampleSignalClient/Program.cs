using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal = new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "signal1" } }
            };

            var id = client.Add(signal).Id.Value;

            client.SetMissingValuePolicy(id, new NoneQualityMissingValuePolicy() { DataType = DataType.Decimal });
            client.SetData(id, new Datum[] { new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1m } });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();

        }
    }
}
