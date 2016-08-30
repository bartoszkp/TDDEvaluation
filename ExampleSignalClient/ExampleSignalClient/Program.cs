using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var id = client.Add(new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "signal" } }
            //}).Id.Value;

            client.SetMissingValuePolicy(1, new ZeroOrderMissingValuePolicy() { DataType = DataType.Double });

            var result = client.GetData(1, new DateTime(2018, 1, 1), new DateTime(2018, 2, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
