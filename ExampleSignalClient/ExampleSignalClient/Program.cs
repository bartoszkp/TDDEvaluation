using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            try
            {
                client.SetData(0, new Datum[0]);
                Console.WriteLine("No exception - WRONG");
            }
            catch (Exception e)
            {
                Console.WriteLine("OK");
            }

            Console.ReadKey();
        }
    }
}
