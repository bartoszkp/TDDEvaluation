using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            client.Add(new Signal() { DataType = DataType.Boolean, Granularity = Granularity.Day, Path = new Path() { Components = new[] { "root", "someSignal" } } });

            client.SetData(1, new Datum[] {
            new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (bool)true },
            new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (bool)false },
            new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (bool)true } });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();
        }
    }
}
