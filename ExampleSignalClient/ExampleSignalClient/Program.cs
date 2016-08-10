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
                var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
            }
            catch (System.ServiceModel.FaultException e)
            {
                Console.WriteLine("Failed to read data", e);
            }

            Console.ReadKey();

        }
    }
}
