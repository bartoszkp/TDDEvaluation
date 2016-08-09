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
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "x" + random.Next(1000), "y" } }
            };

            var fetchedSignal = client.Add(signal);
            client.SetMissingValuePolicy(fetchedSignal.Id.GetValueOrDefault(), new NoneQualityMissingValuePolicy() { DataType = DataType.Double });
            
            client.SetData(fetchedSignal.Id.GetValueOrDefault(), new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
            });


            var result = client.GetData(fetchedSignal.Id.GetValueOrDefault(), new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }


            Console.ReadKey();

        }
    }
}
