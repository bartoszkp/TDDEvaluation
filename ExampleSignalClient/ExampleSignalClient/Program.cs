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
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root/s5" } }
            });


            var data = new Datum[]
            {
                 new Datum() { Quality = Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 4, 26, 0, 0, 0) }
            };

            //client.SetData(1, data);
            client.GetData(1, new DateTime(2000, 4, 26, 0, 0, 0), new DateTime(2000, 4, 26, 0, 0, 0));
            

            Console.ReadKey();
        }
    }
}
