using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var mvp1 = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.String, Quality = Quality.Fair, Value = "x" };
            var mvp2 = new Signals.NoneQualityMissingValuePolicy();

            client.SetMissingValuePolicy(1, mvp1);
            client.SetMissingValuePolicy(2, mvp2);

            var result1 = client.GetMissingValuePolicy(1) as Signals.SpecificValueMissingValuePolicy;
            var result2 = client.GetMissingValuePolicy(2) as Signals.NoneQualityMissingValuePolicy;

            Console.WriteLine(result1.Signal.Id.Value);
            Console.WriteLine(result2.Signal.Id.Value);
            Console.WriteLine(result1.DataType);
            Console.WriteLine(result1.Quality);
            Console.WriteLine(result1.Value);
            Console.ReadKey();
        }
    }
}
