using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            Random random = new Random();

            var id = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "signal" + random.Next(100) } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new SpecificValueMissingValuePolicy() { DataType = DataType.Double, Value = (double)42.42, Quality = Quality.Fair });

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
