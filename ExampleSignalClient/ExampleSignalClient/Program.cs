using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Week,
                Path = new Path() { Components = new[] { "weeklySignal" } }
            }).Id.Value;

            var result = client.GetData(id, new DateTime(2016, 9, 5), new DateTime(2016, 9, 19));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
