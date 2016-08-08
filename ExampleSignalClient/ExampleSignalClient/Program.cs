using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new string[] { "root", "signal1" } }
            });

            var result = client.Get(new Path() { Components = new[] { "bad", "path" } });

            Console.WriteLine(result == null);
            Console.ReadKey();
        }
    }
}
