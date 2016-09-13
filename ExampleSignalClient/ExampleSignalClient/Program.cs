using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal() { DataType = DataType.Boolean, Granularity = Granularity.Day, Path = new Path() { Components = new[] { "7" } } }).Id.Value;

            var result = client.GetById(id);

            if (result != null)
            {
                Console.WriteLine("Sygnał poprawnie utworzony");
            }
            else
            {
                Console.WriteLine("Błąd - nie udało się utworzyć sygnału");
            }

            client.SetMissingValuePolicy(id, new SpecificValueMissingValuePolicy() { DataType = DataType.Boolean, Value = true, Quality = Quality.Good });
            var data = client.GetData(id, new DateTime(2018, 12, 12), new DateTime(2018, 12, 12));

            foreach (var datum in data)
            {
                Console.WriteLine("Datum: " + datum.Quality + " " + datum.Timestamp);
            }

            Console.ReadKey();

        }
    }
}
