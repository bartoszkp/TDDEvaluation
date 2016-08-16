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
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new string[] { "a", "b6" } } };

            int id = client.Add(addedSignal).Id.Value;

            client.SetData(id, new Datum[] {
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                      
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 8, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.WriteLine("end");
            Console.ReadKey();
        }
    }
}
