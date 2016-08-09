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
                DataType = DataType.Decimal,
                Granularity = Granularity.Minute,
                Path = new Path() { Components = new[] { "root", "defaultPolicy222" } }
            };

            var result = client.Get(new Path() { Components = new[] { "root", "defaultPolicy2288" } } );

            Console.WriteLine(result == null);
            Console.ReadKey();
        }
    }
}
