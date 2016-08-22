using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var data = new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 1) }
            };

            client.SetData(1, data);

            Console.ReadKey();

        }
    }
}