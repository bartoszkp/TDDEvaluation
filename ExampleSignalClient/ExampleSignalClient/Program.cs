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
                Granularity = Granularity.Week,
                Path = new Path() { Components = new[] { "FirstOrderTests" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Decimal });

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 8, 29), Value = 1m },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2016, 9, 19), Value = 2m },
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 10, 3), Value = 5m }
            });

            var result = client.GetData(id, new DateTime(2016, 8, 22), new DateTime(2016, 10, 10));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

            //SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var id = client.Add(new Signal()
            //{
            //    DataType = DataType.Integer,
            //    Granularity = Granularity.Second,
            //    Path = new Path() { Components = new[] { "FirstOrderTests1" } }
            //}).Id.Value;

            //client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Integer });

            //client.SetData(id, new Datum[]
            //{
            //new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,1,1), Value = 1},
            //new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,1,5), Value = 2 },
            //new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1,1,1,8), Value = 5 }
            //});

            //var result = client.GetData(id, new DateTime(2000, 1, 1, 1, 1, 6), new DateTime(2000, 1, 1, 1, 1, 7));

            //foreach (var d in result)
            //{
            //    Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            //}

            //Console.ReadKey();



            //  ------------------------------------------------------------------

            //SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var id = client.Add(new Signal()
            //{
            //    DataType = DataType.Decimal,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "FirstOrderTests2" } }
            //}).Id.Value;

            //client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Decimal });

            //client.SetData(id, new Datum[]
            //{
            //new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
            //new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 10, 1), Value = 10m }
            //});

            //var result = client.GetData(id, new DateTime(2000, 3, 1), new DateTime(2000, 6, 1));

            //foreach (var d in result)
            //{
            //    Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            //}

            //Console.ReadKey();


            //          ------------------------------------------------------------------

            //SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var id = client.Add(new Signal()
            //{
            //    DataType = DataType.Integer,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "FirstOrderTests3" } }
            //}).Id.Value;

            //client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Integer});

            //client.SetData(id, new Datum[]
            //{
            //new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 1 },
            //new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2 },
            //new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 5 }
            //});

            //var result = client.GetData(id, new DateTime(1999, 11, 1), new DateTime(2000, 11, 1));

            //foreach (var d in result)
            //{
            //    Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            //}


            //Console.ReadKey();

            //  ------------------------------------------------------------------------------------


            //SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var id = client.Add(new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "FirstOrderTests4" } }
            //}).Id.Value;

            //client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Double });

            //client.SetData(id, new Datum[]
            //{
            //new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2.5 },
            //new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 3.0 },
            //new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2001, 1, 1), Value = 4.5 }
            //});

            //var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2001, 3, 1));

            //foreach (var d in result)
            //{
            //    Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            //}

            //Console.ReadKey();








        }
    }
}
