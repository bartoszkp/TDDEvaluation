using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal() { Granularity = Granularity.Day, Path = new Path() { Components = new[] { String.Empty } } }).Id.Value;

            var result = client.GetById(345);

            if (result != null)
            {
                Console.WriteLine("Sygnał poprawnie utworzony");
            }
            else
            {
                Console.WriteLine("Błąd - nie udało się utworzyć sygnału");
            }

            var data = client.GetData(id, new DateTime(2018, 12, 12), new DateTime(2018, 12, 12));

            foreach (var datum in data)
            {
                Console.WriteLine("Datum: " + datum.Quality + " " + datum.Timestamp);
            }

            Console.ReadKey();
        }
    }
}
