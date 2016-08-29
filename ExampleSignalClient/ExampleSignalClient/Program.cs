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
                DataType = DataType.Decimal,
                Granularity = Granularity.Second,
                Path = new Path() { Components = new[] { "second315778" } }
            }).Id.Value;
            client.SetMissingValuePolicy(id, new ZeroOrderMissingValuePolicy());

            var ts = new DateTime(2000, 1, 1, 1, 1, 1);
            var result = client.GetData(id, ts, ts);

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}

