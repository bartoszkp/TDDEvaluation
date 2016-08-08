using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var signal1 = new Signal() { DataType = DataType.Double, Path = new Path() { Components = new string[] { "A", "B" } }, Granularity = Granularity.Month };
            //client.Add(signal1);

            client.SetData(1, new Datum[] {
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 21), Value = (double)1.5 },
                         new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 2, 13), Value = (double)54 }});

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();
        }
    }
}
