using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Year
            };

            client.Add(signal);

            client.SetMissingValuePolicy(1, new NoneQualityMissingValuePolicy() { DataType = DataType.Double });

            client.SetData(1, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0), Value = (double)1.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2002, 1, 1, 1, 1, 0), Value = (double)1.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2004, 1, 1, 1, 1, 0), Value = (double)2.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2006, 1, 1, 1, 1, 0), Value = (double)2.5 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2007, 1, 1, 1, 1, 0), Value = (double)2.5 }
            });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2008, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
