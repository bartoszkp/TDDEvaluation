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
                Granularity = Granularity.Minute,
                Path = new Path() { Components = new[] { "root", "defaultPolicy515" } }
            };

            int signalId = client.Add(newSignal).Id.Value;

            client.SetMissingValuePolicy(signalId, new NoneQualityMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(signalId, new Datum[]
            {
                    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2003, 1, 1, 1, 1, 1), Value = (double)1.5 },
                    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2003, 1, 1, 1, 3, 1), Value = (double)22 },
                    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2003, 1, 1, 1, 9, 1), Value = (double)5 },
                    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2003, 1, 1, 1, 10, 1), Value = (double)3}
            });

            var result = client.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2007, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }
            Console.ReadKey();
        }
    }
}
