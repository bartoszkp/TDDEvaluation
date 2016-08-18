using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal = client.Add(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Second,
                Path = new Path() { Components = new[] { "signal21" } }
            });

            var result = client.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));

            Console.WriteLine(result.Length);
            Console.ReadKey();


        }
    }
}
