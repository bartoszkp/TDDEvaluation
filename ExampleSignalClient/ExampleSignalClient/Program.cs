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
                Path = new Path() { Components = new[] { "root", "signal36" } }
            };

            var id = client.Add(newSignal).Id.Value;

       



            var mvp = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.Double, Quality = Quality.Fair, Value = (double)1.5 };

            client.SetMissingValuePolicy(100, mvp);

            var result1 = client.GetMissingValuePolicy(1) as Signals.SpecificValueMissingValuePolicy;

            Console.WriteLine(result1.Signal.Id.Value);
            Console.WriteLine(result1.DataType);
            Console.WriteLine(result1.Quality);
            Console.WriteLine(result1.Value);
            Console.ReadKey();
          
        }
    }
}
