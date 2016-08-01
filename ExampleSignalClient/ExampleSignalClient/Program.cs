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
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "root", "dayDecimal1" } }
            };

            //client.Add(newSignal);

            var result = client.Get(new Path() { Components = new[] { "root", "dayDecimal1" } });

            Console.WriteLine(result.Id.Value);
            Console.WriteLine(result.DataType);
            Console.WriteLine(result.Granularity);
            Console.WriteLine(string.Join("/", result.Path.Components));
            Console.ReadKey();
        }
    }
}

