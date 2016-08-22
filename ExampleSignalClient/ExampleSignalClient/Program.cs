using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            
            client.GetData(1, new DateTime(2000,2,2), new DateTime(2000,10,10));

            Console.ReadKey();
        }
    }
}