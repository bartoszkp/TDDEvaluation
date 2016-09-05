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
                DataType = DataType.Decimal,
                Granularity = Granularity.Year,
                Path = new Path() { Components = new[] { "SpecificValue12321" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new SpecificValueMissingValuePolicy() { Value = 11m, Quality = Quality.Bad, DataType = DataType.Decimal });

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 1), Value = 1m }
            });

            var result = client.GetData(id, new DateTime(2017, 1, 1), new DateTime(2019, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
