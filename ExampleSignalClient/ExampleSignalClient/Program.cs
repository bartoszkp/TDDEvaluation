using System;
using ExampleSignalClient.Signals;
using System.Linq;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            var sig = new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "x", "y" } }
            };

            client.Add(sig);

            var data = new Datum[]
            {
                  new Datum() { Quality = Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 1, 12, 45, 0) }
            };

            client.SetData(1, data);

            Console.ReadKey();
        }
    }
}
