using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal { DataType = DataType.Double, Granularity = Granularity.Month }).Id.Value;

            var data = new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Value = 0d, Timestamp = new DateTime(2000, 1, 1, 12, 45, 0) }
            };

            client.SetData(id, data);



            Console.ReadKey();
        }
    }
}
