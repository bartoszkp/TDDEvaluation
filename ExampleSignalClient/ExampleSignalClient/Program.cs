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
                DataType = DataType.Boolean,
                Granularity = Granularity.Day,
                Path = new Path() { Components = new[] { "Bool" } }
            }).Id.Value;

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = false }
            });

            var result = client.GetData(id, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));

            Console.WriteLine(result.Length);
            Console.ReadKey();

        }
    }
}
