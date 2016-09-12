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
            //    DataType = DataType.Integer,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "GetTsTsNoDataTests" } }
            //}).Id.Value;

            var id = 1;

            var result = client.GetData(id, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }
            Console.WriteLine("Done");

            Console.ReadKey();

        }
    }
}
