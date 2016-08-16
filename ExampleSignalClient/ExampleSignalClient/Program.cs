using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var sig = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "defaultPolicy" } }
            };

            client.Add(sig);

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();

        }
    }
}
