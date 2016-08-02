using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            var mvp = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.Double, Quality = Quality.Fair, Value = (double)1.5 };

            //client.SetMissingValuePolicy(2, mvp);

            var result = client.GetMissingValuePolicy(2) as Signals.SpecificValueMissingValuePolicy;

            Console.WriteLine(result.Signal.Id.Value);
            Console.WriteLine(result.DataType);
            Console.WriteLine(result.Quality);
            Console.WriteLine(result.Value);
            Console.ReadKey();
            //SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var newSignal = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "root", "signal12" } }
            //};

            //var id = client.Add(newSignal).Id.Value;

            //var result = client.GetById(id);

            //Console.WriteLine(result.Id);
            //Console.WriteLine(result.DataType);
            //Console.WriteLine(result.Granularity);
            //Console.WriteLine(string.Join("/", result.Path.Components));
            //Console.ReadKey();
        }
    }
}
