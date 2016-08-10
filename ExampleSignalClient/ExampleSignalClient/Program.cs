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
             
            Signal newSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Second,
                Path = new Path() { Components = new[] { "x" + random.Next(1000), "y" } }
            };

            var result = client.Add(newSignal);

            var mvp = client.GetMissingValuePolicy(result.Id.Value);

            Console.WriteLine(mvp);

            Console.ReadKey();

        }
    }
}
