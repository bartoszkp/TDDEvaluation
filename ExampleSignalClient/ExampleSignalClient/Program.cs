using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            client.Delete(1);

            var result = client.GetById(1);

            if (result == null)
            {
                Console.WriteLine("Sygnał skasowany");
            }

            Console.ReadKey();
        }
    }
}
