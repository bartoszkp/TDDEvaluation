using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //client.Add(new Signals.Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month
            //});

            var data = new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Value = (double)0, Timestamp = new DateTime(2000, 1, 1, 12, 45, 0) }
            };

            client.SetData(1, data);

            Console.ReadKey();

        }
    }
}
