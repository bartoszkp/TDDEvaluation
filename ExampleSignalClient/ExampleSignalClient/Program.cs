using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal1 = client.Add(new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Minute,
                Path = new Path() { Components = new[] { "root/s1" } }
            });


            var data = new Datum[]
            {
                 new Datum() { Quality = Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 1, 0, 0, 12) }
            };

            client.SetData(1, data);

            Console.ReadKey();


            Console.ReadKey();
        }
    }
}
