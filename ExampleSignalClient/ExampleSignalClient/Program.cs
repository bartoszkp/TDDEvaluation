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
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "signal1" } }
            }).Id.Value;

            var data = new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0) }
            };

            client.SetData(id, data);

            Console.ReadKey();
        }
    }
}
