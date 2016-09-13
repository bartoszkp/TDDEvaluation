using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var id = client.Add(new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "CoarseTests" } }
            //}).Id.Value;

            var id = 1;

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,1, 1), Value = 1.5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,2, 1), Value = 0.5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,3, 1), Value = 2.76 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,4, 1), Value = 1 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,5, 1), Value = 1 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,6, 1), Value = 1 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,7,1), Value = 1 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016,8,1), Value = 5.4 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,9,1), Value = 5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,10,1), Value = 6.8 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,11,1), Value = 5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016,12,1), Value = 9.9 },

                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,1,1), Value = 2 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,2,1), Value = 1 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,3,1), Value = 2/5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,4,1), Value = 5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,5,1), Value = 5 },
                new Datum() { Quality = Quality.Bad,  Timestamp = new DateTime(2017,6,1), Value = 3.4 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,7,1), Value = 0 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,8,1), Value = 1 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,9,1), Value = 0 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,10,1), Value = 70 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,11,1), Value = 0 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2017,12,1), Value = 0 },

                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,1,1), Value = 2.44 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,2,1), Value = 1 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,3,1), Value = 5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,4,1), Value = 5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,5,1), Value = 3.6 },
                new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2018,6,1), Value = 5 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,7,1), Value = 0 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,8,1), Value = 7.1 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,9,1), Value = 0 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,10,1), Value = 0 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,11,1), Value = 0 },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2018,12,1), Value = 5.12 },
            });

            var result = client.GetCoarseData(id, Granularity.Year, new DateTime(2017, 1, 1), new DateTime(2016, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
