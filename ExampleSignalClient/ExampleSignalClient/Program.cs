using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var newSignal = new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "root", "defaultPolicy" } }
            };

        //    var result1 = client.Add(newSignal);

            client.SetMissingValuePolicy(1, new NoneQualityMissingValuePolicy() { DataType = DataType.Integer });

            client.SetData(1, new Datum[]
            {
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)1 },
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 15), Value = (int)2 },
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = (int)2 }

            });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 2));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
