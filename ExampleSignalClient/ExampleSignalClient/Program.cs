using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var result1 = client.Get(new Path() { Components = new[] { "bad", "path" } } );

            Console.WriteLine(result1 == null);

            var result2 = client.GetById(1000);

            Console.WriteLine(result2 == null);

            Console.ReadKey();

        }
    }
}
