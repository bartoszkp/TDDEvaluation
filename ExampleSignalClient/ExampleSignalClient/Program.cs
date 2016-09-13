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
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "CoarseTestsYear4" } }
            }).Id.Value;

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2016,1, 1), Value = 2 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,2, 1), Value = 3 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,3, 1), Value = 33 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,4, 1), Value = 13 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,5, 1), Value = 11 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,6, 1), Value = 12 },

    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016,7,1), Value = 4 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,8,1), Value = 8 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,9,1), Value = 64 },
    new Datum() { Quality = Quality.None, Timestamp = new DateTime(2016,10,1), Value = 58 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,11,1), Value = 16 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,12,1), Value = 23 },

      new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2017,1, 1), Value = 2 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,2, 1), Value = 3 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,3, 1), Value = 33 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,4, 1), Value = 13 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,5, 1), Value = 11 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,6, 1), Value = 121 },

    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2017,7,1), Value = 4 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,8,1), Value = 8 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,9,1), Value = 64 },
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2017,10,1), Value = 58 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,11,1), Value = 16 },
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,12,1), Value = 23 },
            });

            var result = client.GetCoarseData(id, Granularity.Year, new DateTime(2016, 1, 1), new DateTime(2018, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
