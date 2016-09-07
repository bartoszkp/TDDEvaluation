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
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "SpecificValueTests5" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new SpecificValueMissingValuePolicy() { DataType = DataType.Boolean, Value = true, Quality = Quality.Bad });

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 10), Value = false },
                new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 15), Value = true }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 10), new DateTime(2000, 1, 20));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
