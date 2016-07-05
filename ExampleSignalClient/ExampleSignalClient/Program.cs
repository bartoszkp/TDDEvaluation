using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            // use client to call WebService methods of the Signals module
            // use Console.WriteLine to verify that a desired function works as expected

            Console.ReadKey();
        }
    }
}
