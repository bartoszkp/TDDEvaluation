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

            var newSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "signal1" } }
            };

            var id = client.Add(newSignal).Id.Value;

            newSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "signal2" } },
                Id = id
            };

            try
            {
                client.Add(newSignal);
                Console.WriteLine("Signal with duplicate ID didn't throw - WRONG");
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to add duplicate ID - OK");
            }

            Console.ReadKey();
        }
    }
}
