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
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "signal1" } }
            };

            var id = client.Add(newSignal).Id.Value;

            var result = client.GetById(id);

            Console.WriteLine(result.Id);
            Console.WriteLine(result.DataType);
            Console.WriteLine(result.Granularity);
            Console.WriteLine(string.Join("/", result.Path.Components));

            Console.WriteLine("\n//Data\n");

            client.SetData(1, new Datum[]
            {
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
            });

            var dataResult = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            foreach (var d in dataResult)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.WriteLine("\n//MissingValuePolicy\n");

            var mvp = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.Double, Quality = Quality.Fair, Value = (double)1.5 };

            client.SetMissingValuePolicy(id, mvp);

            var mvpResult = client.GetMissingValuePolicy(id) as Signals.SpecificValueMissingValuePolicy;

            Console.WriteLine(mvpResult.Signal.Id.Value);
            Console.WriteLine(mvpResult.DataType);
            Console.WriteLine(mvpResult.Quality);
            Console.WriteLine(mvpResult.Value);

            Console.WriteLine("\n//GetByPath\n");

            var anotherSignal = new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "root", "dayDecimal1" } }
            };

            client.Add(anotherSignal);

            var anotherResult = client.Get(new Path() { Components = new[] { "root", "dayDecimal1" } });

            Console.WriteLine(anotherResult.Id.Value);
            Console.WriteLine(anotherResult.DataType);
            Console.WriteLine(anotherResult.Granularity);
            Console.WriteLine(string.Join("/", anotherResult.Path.Components));


            Console.ReadKey();
        }
    }
}
