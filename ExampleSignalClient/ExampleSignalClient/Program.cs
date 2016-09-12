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
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "NoneAsDefaultTest2" } }
            }).Id.Value;

            client.GetMissingValuePolicy(id);
        }
    }
}
