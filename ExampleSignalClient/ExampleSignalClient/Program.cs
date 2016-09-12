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
                Path = new Path() { Components = new[] { "ShadowTests" } }
            }).Id.Value;

            var shadow = client.Add(new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Year,
                Path = new Path() { Components = new[] { "shadows", "shadow1" } }
            });


            client.SetData(shadow.Id.Value, new Datum[]
                {
    new Datum() { Quality = Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = false },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2001, 1, 1), Value = true },
    new Datum() { Quality = Quality.None, Timestamp = new DateTime(2002, 1, 1), Value = false },
    new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2003, 1, 1), Value = false },
    new Datum() { Quality = Quality.None, Timestamp = new DateTime(2004, 1, 1), Value = false },
                });

            client.SetMissingValuePolicy(id, new ShadowMissingValuePolicy() { DataType = DataType.Boolean, ShadowSignal = shadow });

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2002, 1, 1), Value = true },
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2005, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
