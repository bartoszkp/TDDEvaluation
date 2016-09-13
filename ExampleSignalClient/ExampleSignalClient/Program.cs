using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal() { DataType = DataType.Integer, Granularity = Granularity.Day, Path = new Path() { Components = new[] { string.Empty } } }).Id.Value;

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = null }
            });

            var data = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 1, 2));

            foreach (var datum in data)
            {
                Console.WriteLine("Datum: " + datum.Quality + " " + datum.Timestamp);
            }

            Console.ReadKey();




        }
    }
}
