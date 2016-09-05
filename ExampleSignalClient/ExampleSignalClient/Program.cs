using System;
using ExampleSignalClient.Signals;
using System.Linq;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            try
            {
                client.Delete(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Nie można usunąć sygnału");
            }

            Console.ReadKey();

        }
    }
}
