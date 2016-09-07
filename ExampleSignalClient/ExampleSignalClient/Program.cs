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
                DataType = DataType.String,
                Granularity = Granularity.Week,
                Path = new Path() { Components = new[] { "ShadowTests11111" } }
            }).Id.Value;

            var shadow = client.Add(new Signal()
            {
                DataType = DataType.String,
                Granularity = Granularity.Week,
                Path = new Path() { Components = new[] { "shadows11111", "shadow121111" } }
            });

            client.SetData(shadow.Id.Value, new Datum[]
            {
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 18), Value = "s1" },
    new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2016, 2, 1), Value = "s2" },
    new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2016, 2, 29), Value = "s3" }
            });

            client.SetMissingValuePolicy(id, new ShadowMissingValuePolicy() { DataType = DataType.String, ShadowSignal = shadow });

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 4), Value = "n1" },
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 2, 1), Value = "n2" },
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 2, 22), Value = "n3" }
            });

            var result = client.GetData(id, new DateTime(2015, 12, 21), new DateTime(2016, 3, 14));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
