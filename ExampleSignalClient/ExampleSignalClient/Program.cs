using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            client.SetData(2, new Datum[] {
                 new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 10.0 },
                 new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 20.0 },
                 new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = 30.0 } });

            var result = client.GetData(2, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();


        }
    }
}
