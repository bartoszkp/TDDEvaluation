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
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "signal1assa" } }
            };

            var id = client.Add(newSignal).Id.Value;

            client.SetData(id, new Datum[0]);

            Console.WriteLine("OK");
            Console.ReadKey();

        }
    }
}
