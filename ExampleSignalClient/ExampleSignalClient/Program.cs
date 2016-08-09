using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var resultPath = client.Get(new Path() { Components = new[] { "bad", "path" } });
            var resultId = client.GetById(-5);

            Console.WriteLine(resultPath == null);
            Console.WriteLine(resultId == null);
            Console.ReadKey();
        }
    }
}
