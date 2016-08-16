using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            Signal addedSignal = new Signal() {
                DataType = DataType.Boolean,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new string[] { "a", "b17" } } };

            int id = client.Add(addedSignal).Id.Value;

            client.SetData(id, new Datum[] {
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = false },
                      
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = true } });

            var result = client.GetData(id, new DateTime(2000, 3, 1), new DateTime(2000, 3, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();
        }
    }
}
