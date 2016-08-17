using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            if (client.GetById(1) == null)
                client.Add(new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Hour,
                    Path = new Path() { Components = new[] { "s0" } }
                });//*/

            //client.SetMissingValuePolicy(1, new SpecificValueMissingValuePolicy() { DataType = DataType.Double, Value = (double)42.42, Quality = Quality.Fair });
            client.SetMissingValuePolicy(1, new NoneQualityMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(1, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = (double)2.5 }
            });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 3));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
