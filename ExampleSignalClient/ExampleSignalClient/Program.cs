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

            var id = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "x" + random.Next(100), "y" + random.Next(100) } }
            }).Id;

            client.SetMissingValuePolicy(id.GetValueOrDefault(), new SpecificValueMissingValuePolicy() { DataType = DataType.Double, Value = (double)42.42, Quality = Quality.Fair });



            client.SetData(id.GetValueOrDefault(), new Datum[] {
                                        new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                                        new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });


            var result = client.GetData(id.GetValueOrDefault(), new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }


            /*
            var signal = client.Add(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Second,
                Path = new Path() { Components = new[] { "ffdsfsd" } }
            });

            var result = client.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));

            Console.WriteLine(result.Length);*/

            Console.ReadKey();


        }
    }
}
