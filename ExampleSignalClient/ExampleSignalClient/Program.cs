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
                Path = new Path() { Components = new[] { "root", "signal1" } }
            };

            var id = client.Add(newSignal).Id.Value;

            var result = client.GetById(id);

            Console.WriteLine(result.Id);
            Console.WriteLine(result.DataType);
            Console.WriteLine(result.Granularity);
            Console.WriteLine(string.Join("/", result.Path.Components));
            Console.ReadKey();
        }
    }
}
