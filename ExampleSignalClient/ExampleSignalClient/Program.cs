using System;
using ExampleSignalClient.Signals;

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
                Path = new Path() { Components = new[] { "root", "defaultPolicy345" } }
            };

            var addedSignal = client.Add(newSignal);

            int id = addedSignal.Id.Value;

            client.SetData(id, new Datum[] {
                         new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 5, 1), Value = (double) 5.5},
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (double) 4.5},
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double) 2.5},
                         new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double) 1.5},
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double) 3.5} });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();
        }
    }
}
