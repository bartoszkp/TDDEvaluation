using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            client.SetData(1, new Datum[] { new Datum() { Timestamp = new DateTime(2000,1,1), Quality=Quality.Bad,  Value=10.0 } } );

            var result = client.GetData(1, new DateTime(1999, 1, 1), new DateTime(2001, 1, 1));

            Console.ReadKey();
        }
    }
}
