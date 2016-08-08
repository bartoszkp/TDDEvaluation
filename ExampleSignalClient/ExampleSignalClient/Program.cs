using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var result = client.Get(new Path() { Components = new[] { "bad", "path" } } );
            var result2 = client.GetById(999543);

            Console.WriteLine(result == null);
            Console.WriteLine(result2 == null);
            Console.ReadKey();
        }
    }
}
