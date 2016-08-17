using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            var existingSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
            };

            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signalId = client.Add(existingSignal).Id;

            client.SetData(signalId.Value, new Datum[] {
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                         new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();


        }
    }
}
