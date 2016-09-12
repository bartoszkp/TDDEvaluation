using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Week,
                Path = new Path() { Components = new[] { "weeklySignal" } }
            }).Id.Value;

            client.SetData(id, new[] { new Datum() { Timestamp = new DateTime(2018, 1, 2), Value = (double)2 } });

            Console.ReadKey();
        }
    }
}
