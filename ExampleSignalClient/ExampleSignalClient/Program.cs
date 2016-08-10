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
                Id = null,
                DataType = DataType.Double,
                Granularity = Granularity.Month,
            };

            client.Add(newSignal);

            var mvp = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.Double, Quality = Quality.Fair, Value = (double)2.0 };

            client.SetMissingValuePolicy(1, mvp);

            var result = client.GetMissingValuePolicy(1) as Signals.SpecificValueMissingValuePolicy;

            Console.WriteLine(result.Signal.Id.Value);
            Console.WriteLine(result.DataType);
            Console.WriteLine(result.Quality);
            Console.WriteLine(result.Value);
            Console.ReadKey();
        }
    }
}
