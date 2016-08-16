using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            
            var signal = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Second,
                Path = new Path() { Components = new[] { "zercrqyv", "zcr" } }
            });

            client.SetData(signal.Id.Value, new Datum[] {
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            var result = client.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 0, 10));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();

        }
    }
}
