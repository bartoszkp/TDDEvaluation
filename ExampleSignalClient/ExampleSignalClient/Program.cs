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
                DataType = DataType.Integer,
                Granularity = Granularity.Minute,
                Path = new Path() { Components = new[] { "root", "defaultPolicy" } }
            };

            var result = client.Add(newSignal);

            var mvp = client.GetMissingValuePolicy(result.Id.Value);

            Console.WriteLine(mvp);

            Console.ReadKey();
        }
    }
}
