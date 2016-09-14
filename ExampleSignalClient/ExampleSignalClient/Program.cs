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
                DataType = DataType.Integer,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "CoarseTests" } }
            }).Id.Value;

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1, 4), Value = 1 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1, 5), Value = 1 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1, 6), Value = 1 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1, 7), Value = 1 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1, 8), Value = 1 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1, 9), Value = 1 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,10), Value = 1 },

    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016,1,11), Value = 5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,12), Value = 5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,13), Value = 5 },
    new Datum() { Quality = Quality.None, Timestamp = new DateTime(2016,1,14), Value = 5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,15), Value = 5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,16), Value = 2 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,17), Value = 1 },

    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,18), Value = 5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,19), Value = 5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,20), Value = 5 },
    new Datum() { Quality = Quality.Bad,  Timestamp = new DateTime(2016,1,21), Value = 5 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,22), Value = 0 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,23), Value = 1 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1,24), Value = 0 },
            });

            var result = client.GetCoarseData(id, Granularity.Week, new DateTime(2016, 1, 4), new DateTime(2016, 1, 25));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
