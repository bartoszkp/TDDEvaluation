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

            Signal signal = new Signal()
            {
                Granularity = Granularity.Month,
                DataType = DataType.Double,
                Path = new Path() { Components = new[] { "x" + random.Next(100), "y" + random.Next(100) } }
            };


            var id = client.Add(signal).Id;
            /*
                        client.SetMissingValuePolicy(id.GetValueOrDefault(), new ZeroOrderMissingValuePolicy() { DataType = DataType.Double });

                        client.SetData(id.GetValueOrDefault(), new Datum[]
                        {
                            new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                            new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                        });

                        var result = client.GetData(id.GetValueOrDefault(), new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                        foreach (var d in result)
                        {
                            Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
                        }*/

            var data = new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 1, 12, 45, 0) }
            };

            client.SetData(id.GetValueOrDefault(), data);



            Console.ReadKey();


        }
    }
}
