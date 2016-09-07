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
                Granularity = Granularity.Week,
                Path = new Path() { Components = new[] { "SpecificValueTests" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new SpecificValueMissingValuePolicy() { DataType = DataType.Boolean, Value = false, Quality = Quality.Bad });

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2018, 1, 1), Value = true }
            });

            var result = client.GetData(id, new DateTime(2018, 1, 8), new DateTime(2018, 1, 15));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
