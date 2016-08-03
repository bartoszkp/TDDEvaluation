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
            int pathNumber = random.Next(1000);

            var newSignal = new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "root" + pathNumber, "dayDecimal1" } }
            };


            client.Add(newSignal);

            var addResult = client.Get(new Path() { Components = new[] { "root" + pathNumber, "dayDecimal1" } });

            int id = addResult.Id.GetValueOrDefault();


            var mvp = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.Double, Quality = Quality.Fair, Value = (double)1.5 };

            client.SetMissingValuePolicy(id, mvp);

            var result = client.GetMissingValuePolicy(id) as Signals.SpecificValueMissingValuePolicy;


            client.SetData(id, new Datum[] {
                              new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                              new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                              new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            var dataResult = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            foreach (var d in dataResult)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.WriteLine(result.Signal.Id.Value);
            Console.WriteLine(result.DataType);
            Console.WriteLine(result.Quality);
            Console.WriteLine(result.Value);
            Console.ReadKey();



        }
    }
}
