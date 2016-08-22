using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            if (client.GetById(1) == null)
            {
                var signal1 = new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = new Path() { Components = new[] { "signal1" } }
                };

                var signalId1 = client.Add(signal1).Id.Value;
            }

            client.SetMissingValuePolicy(1, new ZeroOrderMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(1, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
            });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
