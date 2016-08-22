using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            var random = new Random();
            var newSignal = new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "signal1" + random.Next() } }
            };

            var id = client.Add(newSignal).Id.Value;
            var i = 1;
            client.SetData(id, new Datum[0]);

            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}
